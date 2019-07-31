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
        
        public async Task<DownstreamResponse> Aggregate(List<DownstreamContext> responses)
        {
            var one = await responses[0].DownstreamResponse.Content.ReadAsStringAsync();
            var two = await responses[1].DownstreamResponse.Content.ReadAsStringAsync();
            var merge = $"{one}, {two}";
            //merge = merge.Replace("Hello", "Bye").Replace("{", "").Replace("}", "");
            var headers = responses.SelectMany(x => x.DownstreamResponse.Headers).ToList();
            return new DownstreamResponse(new StringContent(merge), HttpStatusCode.OK, headers, "some reason");
        }
    }
}
