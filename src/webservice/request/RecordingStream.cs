/*
Copyright 2016 Pitney Bowes Inc.

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
        private Stream _baseStream = null;
        private FileStream _recorder = null;
        private FileMode _fileMode;
        private bool _recording;
        /// <summary>
        /// Constructor - just initializes members.
        /// </summary>
        /// <param name="baseStream"></param>
        /// <param name="path"></param>
        /// <param name="fileMode"></param>
        public RecordingStream(Stream baseStream, string path, FileMode fileMode )
        {
            _baseStream = baseStream;
            _path = path;
            _fileMode = fileMode;
            _recording = false;
        }

        /// <summary>
        /// Turns recording on or off. This is a heavy operation - opens and closes the file. If you want to start and stop a lot, modify this method
        /// </summary>
        public bool IsRecording
        {
            get { return _recorder != null && _recording; }
            set { _recording = value; }
        }

        internal void OpenRecord()
        {
            if (_recorder == null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_path));
                _recorder = new FileStream(_path, _fileMode, FileAccess.Write);
            }
        }
        /// <summary>
        /// Close the recording stream, if enabled
        /// </summary>
        public void CloseRecord()
        {
            if ( _recorder != null)
            {
                _recording = false;
#if !NETSTANDARD1_3

                _recorder.Close();
#endif
                _recorder.Dispose();
                _recorder = null;
            }
        }
        /// <summary>
        /// Write to the recording stream if recording enabled
        /// </summary>
        /// <param name="s"></param>
        public void WriteRecord( string s)
        {
            if (_recorder == null) return;
            if (!_recording) return;
            var b = Encoding.UTF8.GetBytes(s);
            _recorder.Write(b, 0, b.Length);
        }
        /// <summary>
        /// Write \\r, \\n to the recording stream 
        /// </summary>
        /// <param name="s"></param>
        public void WriteRecordCRLF(string s)
        {
            if (_recorder == null) return;
            if (!_recording) return;
            WriteRecord(s);
            WriteRecord("\r\n"); 
        }

        /// <summary>
        /// Flush record if recording enabled
        /// </summary>
        public void FlushRecord()
        {
            if (_recorder == null) return;
            _recorder.Flush();

        }
        /// <summary>
        /// Call base.CanRead
        /// </summary>
        public override bool CanRead => _baseStream.CanRead;
        /// <summary>
        /// Call base.CanSeek
        /// </summary>
        public override bool CanSeek => _baseStream.CanSeek;
        /// <summary>
        /// Call base.CanWrite
        /// </summary>
        public override bool CanWrite => _baseStream.CanWrite;
        /// <summary>
        /// Call base.Length
        /// </summary>
        public override long Length => _baseStream.Length;
        /// <summary>
        /// Delegates to base
        /// </summary>
        public override long Position
        {
            get => _baseStream.Position;
            set { _baseStream.Position = value; }
        }
        /// <summary>
        /// Flush base stream
        /// </summary>
        public override void Flush()
        {
            _baseStream.Flush();
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
            return _baseStream.Seek(offset, origin);
        }
        /// <summary>
        /// Call base.SetLength
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
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
            _recorder.Write(buffer, offset, count);
            _baseStream.Write(buffer, offset, count );
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