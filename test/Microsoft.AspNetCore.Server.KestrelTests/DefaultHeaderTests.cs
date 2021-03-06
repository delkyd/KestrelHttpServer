﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.AspNetCore.Server.Kestrel.Internal.Infrastructure;
using Xunit;

namespace Microsoft.AspNetCore.Server.KestrelTests
{
    public class DefaultHeaderTests
    {
        [Fact]
        public async Task TestDefaultHeaders()
        {
            var testContext = new TestServiceContext()
            {
                ServerOptions = { AddServerHeader = true }
            };

            using (var server = new TestServer(ctx => TaskUtilities.CompletedTask, testContext))
            {
                using (var connection = server.CreateConnection())
                {
                    await connection.SendEnd(
                        "GET / HTTP/1.0",
                        "",
                        "");

                    await connection.ReceiveEnd(
                        "HTTP/1.1 200 OK",
                        $"Date: {testContext.DateHeaderValue}",
                        "Server: Kestrel",
                        "",
                        "");
                }
            }
        }
    }
}