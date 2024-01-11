using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameVault
{
    public class SharedViewModel
    {
        public event EventHandler UpdateUI;

        private string _status;

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnSomethingChanged();
            }
        }

        public virtual void OnSomethingChanged()
        {
            UpdateUI?.Invoke(this, EventArgs.Empty);
        }
    }
}
