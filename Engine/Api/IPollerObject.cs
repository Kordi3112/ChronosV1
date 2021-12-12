using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Api
{
    public interface IPollerObject
    {
        //internal int 
        int GetPoolerId();
        void SetPollerId(int id);


        void Delete(bool isDelete);
        bool IsDeleted();
    }
}
