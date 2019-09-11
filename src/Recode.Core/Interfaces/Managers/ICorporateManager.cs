using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Managers
{
    public interface ICorporateManager
    {
        Task<CorporateModel[]> FilterBy(string corporateName);
        Task<CorporateModel> GetById(long corporateId);
        Task<CorporateModel> GetById();
        Task<Page<CorporateModel>> Get(int pageSize, int pageNumber);
        Task<bool> AddFile(FileModel fileModel, long documentId);
        Task<CorporateDocumentModel[]> GetDocument();
        Task<bool> Update(CorporateModel model);
    }
}
