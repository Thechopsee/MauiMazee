using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeUnitTests.Mock
{
    public class MockSecureStorage : ISecureStorage
    {
        private readonly Dictionary<string, string> _data = new Dictionary<string, string>();

        public Task<string> GetAsync(string key)
        {
            if (_data.ContainsKey(key))
                return Task.FromResult(_data[key]);
            return null;
        }

        public Task SetAsync(string key, string value)
        {
            _data[key] = value;
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            if (_data.ContainsKey(key))
                _data.Remove(key);
            return Task.CompletedTask;
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }
    }
}
