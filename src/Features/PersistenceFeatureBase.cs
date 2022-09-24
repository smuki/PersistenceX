using Elsa.Features.Abstractions;
using Elsa.Features.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.Workflows.Persistence.Features
{
    public class PersistenceFeatureBase: FeatureBase
    {
        protected PersistenceFeatureBase(IModule module) : base(module)
        {

        }
    }
}
