using System;
using System.Collections.Generic;
using System.Linq;
using FinancialPortal.Web.Models.Exceptions;
using Newtonsoft.Json;

namespace FinancialPortal.Web.Services.Utility
{
    public class LowellReferenceSurrogateKeyMap
    {
        private readonly Dictionary<Guid, string> _mappings;

        public LowellReferenceSurrogateKeyMap()
        {
            _mappings = new Dictionary<Guid, string>();
        }

        public LowellReferenceSurrogateKeyMap(string serialisedJson)
        {
            if (serialisedJson == null)
                _mappings = new Dictionary<Guid, string>();
            else
                _mappings = JsonConvert.DeserializeObject<Dictionary<Guid, string>>(serialisedJson);
        }

        public string SerialiseAsJson()
        {
            return JsonConvert.SerializeObject(_mappings);
        }

        // Used for logged in behaviour
        // Returns a reverse lookup, used to generate accounts list with surrogate keys for GUIDs
        public IDictionary<string, Guid> AddLowellReferenceSurrogateKeys(IEnumerable<string> lowellReferences)
        {
            Dictionary<string, Guid> reverseLookupDictionary = new Dictionary<string, Guid>();

            foreach (string lowellRef in lowellReferences)
            {
                Guid key = AddLowellReferenceSurrogateKey(lowellRef);
                reverseLookupDictionary.Add(lowellRef, key);
            }

            return reverseLookupDictionary;
        }

        // Used for Anonymous (not logged in) behaviour
        public Guid AddLowellReferenceSurrogateKey(string lowellReference)
        {
            // Only create keys if they don't already exist
            Guid? existingKey = GetSurrogateKeyFromLowellReference(lowellReference);
            if (existingKey.HasValue)
                return existingKey.Value;

            Guid newGuid = Guid.NewGuid();
            _mappings.Add(newGuid, lowellReference);

            return newGuid;
        }

        public string GetLowellReferenceFromSurrogate(Guid surrogateKey)
        {
            // If not found, throw special exception allowing website to redirect
            if (!_mappings.ContainsKey(surrogateKey))
                throw new LowellReferenceSurrogateKeyException("Lowell Reference Surrogate Key not found.");

            return _mappings[surrogateKey];
        }

        // Used for unit testing
        internal int MappingCount => _mappings.Count;

        public Guid? GetSurrogateKeyFromLowellReference(string lowellReference)
        {
            foreach (Guid key in _mappings.Keys)
            {
                if (_mappings[key] == lowellReference)
                    return key;
            }

            return null;
        }

        public string GetTopLowellReference()
        {
            return _mappings?.FirstOrDefault().Value;
        }

        public Guid? GetTopLowellSurrogateKey()
        {
            return _mappings?.FirstOrDefault().Key;
        }
    }
}
