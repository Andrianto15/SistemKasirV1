using SistemKasir.Models;

namespace SistemKasir.Services
{
    public static class GenerateIdServices
    {
        public static string GetID(string prefix, MasterId masterId)
        {
            var lastIdCount = masterId?.Counter;
            var resultId = String.Empty;

            if (lastIdCount != null)
            {
                resultId = prefix + String.Format("{0:D7}", lastIdCount);
            }

            return resultId;
        }
    }
}
