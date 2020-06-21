using LocadoraWebApi.Models;

namespace LocadoraWebApi.Repository
{
    public class BaseRepository
    {
        private ContextoDB _DataModel;
        public ContextoDB DataModel
        {
            get
            {
                if (_DataModel == null)
                    _DataModel = new ContextoDB();
                return _DataModel;
            }
        }
    }
}