using System;
namespace Floofinator.SimpleSave
{
    public interface ISaveable
    {
        public Type GetSaveType();
        public object Save();
        public void Load(object saveData);
    }
}