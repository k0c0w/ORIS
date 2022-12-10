using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TapChat
{
    public record class User
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public Color Color { get; init; }
    }
}
