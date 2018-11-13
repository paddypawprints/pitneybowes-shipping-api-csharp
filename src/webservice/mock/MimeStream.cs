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

namespace PitneyBowes.Developer.ShippingApi
{
    internal class MimeStream : Stream
    {
        Stream _stream;
        StreamEnumerator _reader;
        byte[] _line = new byte[0];
        int _lineOffset = 0;
        string _boundary = "";
        bool _atBoundary = false;
        long _partStreamStart;
        bool _isMultipart = false;
        public string FirstLine;
        bool _endOfFile = false;
        Dictionary<string, List<string>> _headers;
        public Dictionary<string, List<string>> Headers
        {
            get => _headers;
        }
        public void ClearHeaders()
        {
            _headers = new Dictionary<string, List<string>>();
        }
        public void MatchFirstLine()
        {

        }
        string ReadLine()
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
                    first = false;
                    current = c;
                }
                else 
                {
                    prev = current;
                    current = c;
                    if (prev == '\r' && current == '\n')
                    {
                        sb.Remove(sb.Length - 1, 1);
                        return sb.ToString();
                    }
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        public void ReadHeaders()
        {
            bool firstLine = true;
            string l;
            while (!_reader.IsEndOfStream)
            {
                l = ReadLine();
                if (l.Equals(""))
                {
                    _partStreamStart = _reader.Offset;
                    break;
                }
                if (firstLine)
                {
                    firstLine = false;
                    FirstLine = l;
                    // HTTP hack - read http headers as well - http first line is not a header
                    if (l.StartsWith("HTTP") || l.StartsWith("GET")|| l.StartsWith("PUT")|| l.StartsWith("POST")|| l.StartsWith("DELETE")|| l.StartsWith("PATCH")) 
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
                    _boundary = _headers["Content-Type"][1].Split('"')[1];
                }
            }
        }


        public void SeekNextPart()
        {
            if (!_isMultipart) return;
            if (_atBoundary) 
            {
                _atBoundary = false;
            }
            string l = "";
            while (!_reader.IsEndOfStream)
            {
                l = ReadLine();
                if (l.StartsWith("--" + _boundary))
                {
                    break;
                }
            }
            if (l.Equals("--" + _boundary + "--"))
            {
                _endOfFile = true;
                return;
            }
            _partStreamStart = _reader.Offset;
            _headers = new Dictionary<string, List<string>>();
            ReadHeaders();
            _endOfFile = false;
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotImplementedException();

        public override long Position { get => _stream.Position - _partStreamStart; set => throw new NotImplementedException(); }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_endOfFile)
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
                    buffer[offset + i] = _line[_lineOffset + i];
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
                    if (line.StartsWith("--"+_boundary))
                    {
                        _atBoundary = true;
                        _endOfFile = true; 
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

        public override long Seek(long offset, SeekOrigin origin)
        {
            if ( origin == SeekOrigin.Begin && offset == 0 )
            {
                _stream.Seek(_partStreamStart, SeekOrigin.Begin);
                _reader.OnSeek();
                _endOfFile = false;
                _lineOffset = 0;
                _line = new byte[0];
                return _stream.Position - _partStreamStart;
            }
            else
            {
                throw new NotImplementedException();
            }

        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
        public new void Dispose()
        {
             _stream.Dispose();
        }

    }
}
