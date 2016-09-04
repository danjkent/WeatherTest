using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherTest.WebUI.Models
{
    public abstract class Disposable : IDisposable
    {
        #region Constants

        #endregion

        #region Constructors

        #endregion

        #region Events + Delegates

        #endregion

        #region Properties

        #endregion

        #region Methods

        #endregion
        
        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
