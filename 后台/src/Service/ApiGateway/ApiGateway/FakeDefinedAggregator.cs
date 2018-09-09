using Ocelot.Middleware;
using Ocelot.Middleware.Multiplexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SourcePoint.Service.ApiGateway
{
    public class FakeDefinedAggregator : IDefinedAggregator
    {

        public FakeDefinedAggregator()
        {
        }

        public async Task<DownstreamResponse> Aggregate(List<DownstreamResponse> responses)
        {
            var one = await responses[0].Content.ReadAsStringAsync();
            var two = await responses[1].Content.ReadAsStringAsync();
            var merge = $"{one}, {two}";
            //merge = merge.Replace("Hello", "Bye").Replace("{", "").Replace("}", "");
            var headers = responses.SelectMany(x => x.Headers).ToList();
            return new DownstreamResponse(new StringContent(merge), HttpStatusCode.OK, headers);
        }
    }
}
