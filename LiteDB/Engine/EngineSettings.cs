﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace LiteDB.Engine
{
    /// <summary>
    /// All engine settings used to starts new engine
    /// </summary>
    public class EngineSettings
    {
        /// <summary>
        /// Get/Set custom stream to be used as datafile (can be MemoryStrem or TempStream). Do not use FileStream - to use physical file, use "filename" attribute (and keep DataStrem/WalStream null)
        /// </summary>
        public Stream DataStream { get; set; } = null;

        /// <summary>
        /// Get/Set custom stream to be used as log file. If is null, use a new TempStream (for TempStrem datafile) or MemoryStrema (for MemoryStream datafile)
        /// </summary>
        public Stream LogStream { get; set; } = null;

        /// <summary>
        /// Get/Set custom instance for Logger
        /// </summary>
        public Logger Log { get; set; } = null;

        /// <summary>
        /// Full path or relative path from DLL directory. Can use ':temp:' for temp database or ':memory:' for in-memory database. (default: null)
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Get database password to decrypt pages
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Timeout for waiting unlock operations (default: 1 minute)
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(1);

        /// <summary>
        /// If database is new, initialize with allocated space (in bytes) (default: 0)
        /// </summary>
        public long InitialSize { get; set; } = 0;

        /// <summary>
        /// Max limit of datafile (in bytes) (default: MaxValue)
        /// </summary>
        public long LimitSize { get; set; } = long.MaxValue;

        /// <summary>
        /// Debug messages from database - (default: Logger.NONE)
        /// </summary>
        public byte LogLevel { get; set; } = Logger.NONE;

        /// <summary>
        /// Returns date in UTC timezone from BSON deserialization (default: false == LocalTime)
        /// </summary>
        public bool UtcDate { get; set; } = false;

        /// <summary>
        /// Indicate that engine will do a checkpoint on dispose database
        /// </summary>
        public bool CheckpointOnShutdown { get; set; } = false;

        /// <summary>
        /// Indicate that engine will open files in readonly mode (and will not support any database change)
        /// </summary>
        public bool ReadOnly { get; set; } = false;

        /// <summary>
        /// Get Data/Log Stream factory
        /// </summary>
        internal IStreamFactory GetStreamFactory(DbFileMode filemode)
        {
            var stream = filemode == DbFileMode.Datafile ? this.DataStream : this.LogStream;
            var filename = filemode == DbFileMode.Datafile ? this.Filename : FileHelper.GetLogFile(this.Filename);

            if (stream != null)
            {
                return new StreamFactory(stream, filemode);
            }
            else if (filename == ":memory:")
            {
                return new StreamFactory(new MemoryStream(), filemode);
            }
            else if (filename == ":temp:")
            {
                return new StreamFactory(new TempStream(), filemode);
            }
            else if (!string.IsNullOrEmpty(filename))
            {
                return new FileStreamFactory(filename, filemode, this.ReadOnly);
            }

            throw new ArgumentException("EngineSettings must have Filename or DataStream as data source");
        }
    }
}