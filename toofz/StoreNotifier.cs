﻿using System;
using System.Diagnostics;
using log4net;

namespace toofz
{
    public sealed class StoreNotifier : NotifierBase
    {
        public StoreNotifier(ILog log, string name) : base("Store", log, name)
        {
            Progress = new ActionProgress<long>(r => rowsAffected = r);
        }

        long rowsAffected;

        public IProgress<long> Progress { get; }
        public long RowsAffected => rowsAffected;

        #region IDisposable Implementation

        bool disposed;

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                var rows = RowsAffected;
                var total = Stopwatch.Elapsed.TotalSeconds;
                var rate = (rows / total).ToString("F0");

                Log.Info($"{Category} {Name} complete -- {rows} rows affected over {total.ToString("F1")} seconds ({rate} rows per second).");
            }

            disposed = true;
            base.Dispose(disposing);
        }

        #endregion
    }
}
