using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CloudPlayer.Models
{
    public class SimpleFile
    {
        public SimpleFile(string Name, Stream Stream)
        {
            this.Name = Name;
            this.Stream = Stream;
        }
        public string Name { get; set; }
        public Stream Stream { get; set; }
    }

    public class SimpleFileAbstraction : TagLib.File.IFileAbstraction
    {
        private SimpleFile file;

        public SimpleFileAbstraction(SimpleFile file)
        {
            this.file = file;
        }

        public string Name
        {
            get { return file.Name; }
        }

        public System.IO.Stream ReadStream
        {
            get { return file.Stream; }
        }

        public System.IO.Stream WriteStream
        {
            get { return file.Stream; }
        }

        public void CloseStream(System.IO.Stream stream)
        {
            stream.Position = 0;
        }
    }

    public class AudioTagHelper
    {
        public static TagLib.Tag FileTagReader(Stream stream, string fileName)
        {
            //Create a simple file and simple file abstraction
            var simpleFile = new SimpleFile(fileName, stream);
            var simpleFileAbstraction = new SimpleFileAbstraction(simpleFile);
            /////////////////////

            //Create a taglib file from the simple file abstraction
            var mp3File = TagLib.File.Create(simpleFileAbstraction);

            //Get all the tags
            TagLib.Tag tags = mp3File.Tag;


            mp3File.Dispose();

            //Return the tags
            return tags;
        }
    }
}
