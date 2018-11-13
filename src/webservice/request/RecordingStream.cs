/*
Copyright 2018 Pitney Bowes Inc.

Licensed under the MIT License(the "License"); you may not use this file except in compliance with the License.  
You may obtain a copy of the License in the README file or at
   https://opensource.org/licenses/MIT 
Unless required by applicable law or agreed to in writing, software distributed under the License is distributed 
on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the License 
for the specific language governing permissions and limitations under the License.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS 
OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

*/

using System;
using System.IO;
using System.Text;

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Whatever is read or written to the stream is recorded in the file
    /// </summary>
    public sealed class RecordingStream : Stream, IDisposable
    {
        private string _path;
        private FileStream _recorder = null;
        private FileMode _fileMode;
        private bool _recording;
        private string _mimeBoundary = null;
        private RecordType _recordType = RecordType.PlainText;
        private bool _wroteMimeHeader = false;
        private Stream _baseStream = null;

        /// <summary>
        /// Record either plain text or multipart mime.
        /// </summary>
        public enum RecordType {
            /// <summary>
            /// multipart MIME.
            /// </summary>
            MultipartMime,
            /// <summary>
            /// plain text.
            /// </summary>
            PlainText
        }
        /// <summary>
        /// Gets the base stream.
        /// </summary>
        /// <value>The base stream.</value>
        public Stream BaseStream {
            get => _baseStream;
        }
        /// <summary>
        /// Gets or sets the base stream and the content type of the stream.
        /// </summary>
        /// <value>The base stream.</value>
        public void SetBaseStream( Stream stream, string contentType = "text/plain" ) 
        {

            if (_baseStream != null)
            {
                _baseStream.Flush();
                _baseStream.Close();
            }
            _baseStream = stream;
            if (_baseStream != null )
                WriteMimeBoundary( contentType );

        }

        /// <summary>
        /// Constructor - just initializes members.
        /// </summary>
        /// <param name="baseStream"></param>
        /// <param name="path"></param>
        /// <param name="fileMode"></param>
        /// <param name="recordType"></param>
        public RecordingStream(Stream baseStream, string path, FileMode fileMode, RecordType recordType)
        {
            _recordType = recordType;
            _baseStream = baseStream;
            _path = path;
            _fileMode = fileMode;
            _recording = false;
        }

        private void WriteMimeHeader()
        {
            if (!IsRecording || _recordType != RecordType.MultipartMime ) return;
            if (!_wroteMimeHeader) 
            {
                if (_mimeBoundary == null )
                {
                    _mimeBoundary = Guid.NewGuid().ToString();
                }
                WriteRecordCRLF("MIME-Version: 1.0");
                WriteRecord("Content-Type: multipart/mixed;boundary = \"");
                WriteRecord(_mimeBoundary);
                WriteRecordCRLF("\"");
                WriteRecordCRLF("");
                WriteRecordCRLF("This is a multipart message in MIME format.");
                WriteRecordCRLF("");
            }
            _wroteMimeHeader = true;
        }

        private void WriteMimeBoundary(string contentType )
        {
            if (!IsRecording || _recordType != RecordType.MultipartMime) return;

            WriteRecordCRLF("");
            WriteRecord("--");
            WriteRecordCRLF(_mimeBoundary);
            WriteRecordCRLF("MIME-Version: 1.0");
            WriteRecord("Content-Type: ");
            WriteRecordCRLF(contentType);
            WriteRecordCRLF("");

        }

        private void WriteMimeClose()
        {
            if (!IsRecording || _recordType != RecordType.MultipartMime) return;

            WriteRecordCRLF("");
            WriteRecord("--");
            WriteRecord(_mimeBoundary);
            WriteRecordCRLF("--");
        }


        /// <summary>
        /// Turns recording on or off. 
        /// </summary>
        public bool IsRecording
        {
            get => _recording; 
            set 
            {
                if (value && _recorder == null)
                {
                    _recording = false;
                }
                if (!_recording && value ) {
                    _recording = value;
                    WriteMimeHeader();
                }
                else
                    _recording = value;
            }
        }
        /// <summary>
        /// Opens the recording file.
        /// </summary>
        /// <param name="startRecording">If set to <c>true</c> start recording.</param>
        public void OpenRecord(bool startRecording )
        {
            if (_recorder == null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_path));
                _recorder = new FileStream(_path, _fileMode, FileAccess.Write);
            }
            IsRecording = startRecording;
        }
        /// <summary>
        /// Close the recording stream, if enabled
        /// </summary>
        public void CloseRecord()
        {
            if (_recorder != null)
            {
                WriteMimeClose();
                _recording = false;
                _recorder.Close();
                _recorder.Dispose();
                _recorder = null;
            }
        }
        /// <summary>
        /// Write to the recording stream if recording enabled
        /// </summary>
        /// <param name="s"></param>
        public void WriteRecord(string s)
        {
            if (!IsRecording) return;
            var b = Encoding.UTF8.GetBytes(s);
            _recorder.Write(b, 0, b.Length);
        }
        /// <summary>
        /// Write \\r, \\n to the recording stream 
        /// </summary>
        /// <param name="s"></param>
        public void WriteRecordCRLF(string s)
        {
            WriteRecord(s);
            WriteRecord("\r\n");
        }

        /// <summary>
        /// Flush record if recording enabled
        /// </summary>
        public void FlushRecord()
        {
            if (!IsRecording) return;
            _recorder.Flush();

        }
        /// <summary>
        /// Call base.CanRead
        /// </summary>
        public override bool CanRead 
        {
            get { if (_baseStream != null) { return _baseStream.CanRead; } else return false; }
        }
        /// <summary>
        /// Call base.CanSeek
        /// </summary>
        public override bool CanSeek
        {
            get { if (_baseStream != null) { return _baseStream.CanSeek; } else return false; }
        }
        /// <summary>
        /// Call base.CanWrite
        /// </summary>
        public override bool CanWrite
        {
            get { if (_baseStream != null) { return _baseStream.CanWrite; } else return false; }
        }
        /// <summary>
        /// Call base.Length
        /// </summary>
        public override long Length
        {
            get { if (_baseStream != null) { return _baseStream.Length; } else return -1; }
        }
        /// <summary>
        /// Delegates to base
        /// </summary>
        public override long Position
        {
            get { if (_baseStream != null) { return _baseStream.Position; } else return -1; }
            set { if (_baseStream != null) { _baseStream.Position = value; } }
        }
        /// <summary>
        /// Flush base stream
        /// </summary>
        public override void Flush()
        {
            if (_baseStream != null) _baseStream.Flush();
        }
        /// <summary>
        /// Call read on base and write what was written to the recording stream, if enabled
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_baseStream == null) return 0;
                int r = _baseStream.Read( buffer, offset, count);
            if (IsRecording && r > 0)
                _recorder.Write(buffer, offset, r);
            return r;
        }
        /// <summary>
        /// Call base.Seek
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            if (_baseStream == null) return 0;
            return _baseStream.Seek(offset, origin);
        }
        /// <summary>
        /// Call base.SetLength
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            if (_baseStream == null) return;
            _baseStream.SetLength(value);
        }

        /// <summary>
        /// Write to base stream and write to recording file
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_baseStream == null) return;
            if (IsRecording )
                _recorder.Write(buffer, offset, count);
            _baseStream.Write(buffer, offset, count );
        }
        /// <summary>
        /// Close the underlying stream.
        /// </summary>
        public override void Close()
        {
            if (_baseStream == null) return;
            _baseStream.Close();

        }
        /// <summary>
        /// Close the recording stream, call base.Dispose
        /// </summary>
        public new void Dispose()
        {
            CloseRecord();
            base.Dispose();
        }

    }
}