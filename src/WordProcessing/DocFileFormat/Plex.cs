/*
 * Copyright (c) 2008, DIaLOGIKa
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *        notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of DIaLOGIKa nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY DIaLOGIKa ''AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL DIaLOGIKa BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using DIaLOGIKa.b2xtranslator.StructuredStorage.Reader;

namespace DIaLOGIKa.b2xtranslator.DocFileFormat
{
    public class Plex<T>
    {
        protected const int CP_LENGTH = 4;

        public List<int> CharacterPositions;
        public List<T> Elements;

        public Plex(int structureLength, VirtualStream tableStream, uint fc, uint lcb)
        {
            tableStream.Seek((long)fc, System.IO.SeekOrigin.Begin);
            var reader = new VirtualStreamReader(tableStream);

            int n = 0;
            if(structureLength > 0)
            {
                //this PLEX contains CPs and Elements
                n = ((int)lcb - CP_LENGTH) / (structureLength + CP_LENGTH);
            }
            else
            {
                //this PLEX only contains CPs
                n = ((int)lcb - CP_LENGTH) / CP_LENGTH;
            }

            //read the n + 1 CPs
            this.CharacterPositions = new List<int>();
            for (int i = 0; i < n + 1; i++)
            {
                this.CharacterPositions.Add(reader.ReadInt32());
            }

            //read the n structs
            this.Elements = new List<T>();
            var genericType = typeof(T);
            if (genericType == typeof(short))
            {
                this.Elements = new List<T>();
                for (int i = 0; i < n; i++)
                {
                    var value = reader.ReadInt16();
                    var genericValue = (T)Convert.ChangeType(value, typeof(T));
                    this.Elements.Add(genericValue);
                }
            }
            else if(structureLength > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    var constructor = genericType.GetConstructor(new Type[] { typeof(VirtualStreamReader), typeof(int) });
                    var value = constructor.Invoke(new object[] { reader, structureLength });
                    var genericValue = (T)Convert.ChangeType(value, typeof(T));
                    this.Elements.Add(genericValue);
                }
            }
            
        }

        /// <summary>
        /// Retruns the struct that matches the given character position.
        /// </summary>
        /// <param name="cp">The character position</param>
        /// <returns>The matching struct</returns>
        public T GetStruct(int cp)
        {
            int index = -1;
            for (int i = 0; i < this.CharacterPositions.Count; i++)
            {
                if (this.CharacterPositions[i] == cp)
                {
                    index = i;
                    break;
                }
            }

            if (index >= 0 && index < this.Elements.Count)
            {
                return this.Elements[index];
            }
            else
            {
                return default(T);
            }
        }
    }
}
