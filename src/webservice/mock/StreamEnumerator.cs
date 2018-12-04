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
using System.Collections;
using System.Collections.Generic;
namespace PitneyBowes.Developer.ShippingApi.Mock
{
	internal class StreamEnumerator 
    {

        private readonly byte[] _buffer = new byte[1024];
        private int _size = 0;
        public long Offset = 0;
        private Stream _stream;
        private bool _eof = false;

        public void OnSeek()
        {
            Offset = 0;
            _size = 0;
            _eof = false;
        }
        public bool IsEndOfStream 
        {
            get => _eof;
        }
        public StreamEnumerator( Stream stream )
        {
            _stream = stream;
        }

        public IEnumerator<byte> ByteEnumerator()
        {
            while (true)
            {
                if (Offset % 1024 == 0)
                {
                    _size = _stream.Read(_buffer, 0, 1024);
                    if (_size == 0)
                    {
                        break;
                    }
                }
                if (Offset % 1024 == _size ) // only happens if size < 1024
                {
                    break;
                }
                var b = _buffer[Offset % 1024];
                Offset++;
                yield return b;
            }
            _eof = true;
        }

        public IEnumerator<char> CharEnumerator()
        {
            var ue = Encoding.GetEncoding("utf-8");
            byte[] buffer = new byte[4];
            int byteCount = 0;
            char[] output = new char[1];

            var bi = ByteEnumerator();
            while ( bi.MoveNext() )
            {
                // Get corrent number of chars for utf-8
                buffer[0] = bi.Current;
                byteCount = 1;
                if ( (buffer[0] & 0x80) != 0x00 ) 
                {
                    bi.MoveNext();
                    buffer[1] = bi.Current;
                    byteCount = 2;
                    if ((buffer[0] & 0xD0) != 0xC0 )
                    {
                        bi.MoveNext();
                        buffer[2] = bi.Current;
                        byteCount = 3;
                        if ((buffer[0] & 0xF0) != 0xeE0) {
                            bi.MoveNext();
                            buffer[3] = bi.Current;
                            byteCount = 4;
                        }
                    }
                }
                if (ue.GetChars(buffer, 0, byteCount, output, 0) != 1 )
                {
                    //TODO: something
                }
                yield return output[0];
            }
        }
    }
}
