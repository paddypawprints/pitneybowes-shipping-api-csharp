// /*
// Copyright 2016 Pitney Bowes Inc.
//
// Licensed under the MIT License(the "License"); you may not use this file except in compliance with the License.  
// You may obtain a copy of the License in the README file or at
//    https://opensource.org/licenses/MIT 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed 
// on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the License 
// for the specific language governing permissions and limitations under the License.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS 
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// */
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace PitneyBowes.Developer.ShippingApi.Mock
{
    /// <summary>
    /// Implements a stream class that provides stream access to the body of a particular part of a MIME message. Methods are provided to access the MIME 
    /// headers and navigate to the required part.
    /// </summary>
    public class MimeStream : Stream
    {
        readonly Stream _stream;
        readonly string _boundaryToken = "";
        readonly bool _isMultipart = false;

        StreamEnumerator _reader;
        byte[] _line = new byte[0];
        int _lineOffset = 0;
        bool _atPartBoundary = false;
        long _partStreamBegin;
        /// <summary>
        /// The first line of the mome stream. The call to ReadHeaders can be used to read http headers of a MIME part as well as MIME headers. If this is done then the first line of the 
        /// http part is the protocol information.
        /// </summary>
        public string FirstLine;
        bool _endOfFile = false;
        Dictionary<string, List<string>> _headers;
        /// <summary>
        /// MIME or http headers.
        /// </summary>
        /// <value>The headers.</value>
        public Dictionary<string, List<string>> Headers
        {
            get => _headers;
        }
        /// <summary>
        /// Clears the headers in preparation for the next read. If you have a reference to the old headers it will still be good. ClearHeaders allocates a new object.
        /// </summary>
        public void ClearHeaders()
        {
            _headers = new Dictionary<string, List<string>>();
        }

        private string ReadLine()
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            char prev = ' ';
            char current= ' ';

            char c;
            var ci = _reader.CharEnumerator();
            while (ci.MoveNext())
            {
                c = ci.Current;
                if (first)
                {
                    if ( c == '\n' )
                    {
                        return sb.ToString();
                    }
                    first = false;
                    current = c;
                }
                else 
                {
                    prev = current;
                    current = c;
                    if (current == '\n')
                    {
                        if (prev == '\r')
                        {
                            sb.Remove(sb.Length - 1, 1);
                        }
                        return sb.ToString();
                    }
                }
                sb.Append(c);
                if (sb.ToString().EndsWith( "0093", StringComparison.InvariantCulture))
                {
                    // debug
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// Reads the current set of MIME or http headers. Assumes the underlying stream is positioned at the start of the header block.
        /// </summary>
        public void ReadHeaders()
        {
            bool firstLine = true;
            string l;
            while (!_reader.IsEndOfStream)
            {
                l = ReadLine();
                if (l.Equals(""))
                {
                    _partStreamBegin = _reader.Offset;
                    break;
                }
                if (firstLine)
                {
                    firstLine = false;
                    FirstLine = l;
                    // HTTP hack - read http headers as well - http first line is not a header
                    if (l.StartsWith("HTTP",StringComparison.InvariantCulture) 
                        || l.StartsWith("GET", StringComparison.InvariantCulture) 
                        || l.StartsWith("PUT", StringComparison.InvariantCulture) 
                        || l.StartsWith("POST", StringComparison.InvariantCulture) 
                        || l.StartsWith("DELETE", StringComparison.InvariantCulture) 
                        || l.StartsWith("PATCH", StringComparison.InvariantCulture)) 
                    {
                        continue;
                    }
                }
                var m = l.Split(':');
                var header = m[0].Trim();
                var values = new List<string>();
                if (m.Length > 1)
                {
                    foreach (var v in m[1].Split(';'))
                    {
                        values.Add(v.Trim());
                    }

                }
                _headers.Add(header, values);
            }
        }
        /// <summary>
        /// Create a MIME stream based on an underlying stream. Provides methods to work with parts of the MIME stream and gives stream access to MIME body parts.
        /// </summary>
        /// <param name="stream">The underlying stream.</param>
        public MimeStream(Stream stream)
        {
            _stream = stream;
            _reader = new StreamEnumerator(stream);
            _headers = new Dictionary<string, List<string>>();

            ReadHeaders();
            if (_headers.ContainsKey("Content-Type"))
            {
                if (_headers["Content-Type"].Count > 1 && _headers["Content-Type"][0] == "multipart/mixed")
                {
                    _isMultipart = true;
                    _boundaryToken = _headers["Content-Type"][1].Split('"')[1];
                }
            }
        }

        /// <summary>
        /// Seeks the next part of the MIME stream.
        /// </summary>
        public void SeekNextPart()
        {
            if (!_isMultipart) return;
            if (_endOfFile) return;
            if (_atPartBoundary) 
            {
                _atPartBoundary = false;
                return;
            }
            string l = "";
            while (!_reader.IsEndOfStream)
            {
                l = ReadLine();
                if (l.StartsWith("--" + _boundaryToken, StringComparison.InvariantCulture))
                {
                    break;
                }
            }
            if (l.Equals("--" + _boundaryToken + "--"))
            {
                _endOfFile = true;
                return;
            }
            _partStreamBegin = _reader.Offset;
            _headers = new Dictionary<string, List<string>>();
            ReadHeaders();
            _endOfFile = false;
        }
        /// <summary>
        /// Returns true
        /// </summary>
        /// <value><c>true</c></value>
        public override bool CanRead => true;
        /// <summary>
        /// Whether the underlying stream can seek.
        /// </summary>
        /// <value><c>true</c> if underlying stream can seek; otherwise, <c>false</c>.</value>
        public override bool CanSeek => _stream.CanSeek;
        /// <summary>
        /// Returns false
        /// </summary>
        /// <value><c>false</c></value>
        public override bool CanWrite => false;
        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <value>Throws NotImplementedException</value>
        public override long Length => throw new NotImplementedException();
        /// <summary>
        /// Gets the position within the current body part. Set is not implemented.
        /// </summary>
        /// <value>The position.</value>
        public override long Position { get => _stream.Position - _partStreamBegin; set => throw new NotImplementedException(); }
        /// <summary>
        /// Not implemented.
        /// </summary>
        public override void Flush()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Provides a stream read method for the current body part.
        /// </summary>
        /// <returns>The read.</returns>
        /// <param name="buffer">Buffer.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="count">Count.</param>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_endOfFile|| _atPartBoundary)
            {
                return 0;
            }
            int total = 0;
            while (true)
            {
                var toCopy = Math.Min(_line.Length - _lineOffset, count);
                int i = 0;
                for (i = 0; i < toCopy; i++)
                {
                    buffer[offset + i + total] = _line[_lineOffset + i];
                }
                total += i;
                count -= i;
                _lineOffset += i;
                if (count > 0)
                {
                    if (_reader.IsEndOfStream)
                    {
                        return total;
                    }
                    var line = ReadLine();
                    if (line.Contains("CLIENT_FACILITY_ID"))
                    {
                        // debug
                    }
                    if (line.StartsWith("--"+_boundaryToken))
                    {
                        _partStreamBegin = _reader.Offset;
                        _atPartBoundary = true;
                        if (line.Equals("--" + _boundaryToken + "--"))
                        {
                            _endOfFile = true;
                        }
                        return total;
                    }
                    _line = Encoding.UTF8.GetBytes(line);
                    _lineOffset = 0;
                    continue;
                }
                else
                {
                    return total;
                }
            }
        }
        /// <summary>
        /// Provides a seek operation in the current body part. Only SeekOrigin.Begin and offset == 0 is supported.
        /// </summary>
        /// <returns>The seek.</returns>
        /// <param name="offset">Offset.</param>
        /// <param name="origin">Origin.</param>
        public override long Seek(long offset, SeekOrigin origin)
        {
            if ( origin == SeekOrigin.Begin && offset == 0 )
            {
                _stream.Seek(_partStreamBegin, SeekOrigin.Begin);
                _reader.OnSeek();
                _atPartBoundary = false;
                _endOfFile = false;
                _lineOffset = 0;
                _line = new byte[0];
                return _stream.Position - _partStreamBegin;
            }
            else
            {
                throw new NotImplementedException();
            }

        }
        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="value">NotImplementedException</param>
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="count">Count.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Does nothing. Dispose the underlying stream yourself. This is deliberate. This class does not manage the underlying stream.
        /// </summary>
        public new void Dispose()
        {
        }

    }
}
