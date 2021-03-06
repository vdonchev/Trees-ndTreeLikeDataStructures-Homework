﻿namespace _02.TraverseАndSaveDirectory
{
    using System.Collections.Generic;

    public class Folder
    {
        private long size;

        public Folder(string name)
        {
            this.Name = name;

            this.Files = new List<File>();
            this.Folders = new List<Folder>();
        }

        public string Name { get; private set; }

        public IList<File> Files { get; private set; }

        public IList<Folder> Folders { get; private set; }

        public long Size
        {
            get
            {
                // Return folder size if is already calculated
                if (this.size != 0 || (this.Files.Count == 0 && this.Folders.Count == 0))
                {
                    return this.size;
                }

                foreach (var file in this.Files)
                {
                    this.size += file.Size;
                }

                foreach (var subFolder in this.Folders)
                {
                    // Calculate each subFolder size recursively
                    this.size += subFolder.Size;
                }

                return this.size;
            }
        }
    }
}