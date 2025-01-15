using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunctionBuilder.ViewModels;

namespace FunctionBuilder.Abstract
{
    public interface IClipBoardService
    {
        Task<IEnumerable<PointViewModel>> Fetch();
        Task<bool> Put(IEnumerable<PointViewModel> data);
    }
}