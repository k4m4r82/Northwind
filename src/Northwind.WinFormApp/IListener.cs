using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Northwind.WinFormApp
{
    public interface IListener
    {
        void Ok(object sender, bool isNewData, object data);
    }
}
