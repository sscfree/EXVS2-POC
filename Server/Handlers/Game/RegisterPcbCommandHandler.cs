﻿using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record RegisterPcbCommand(Request Request, string BaseAddress) : IRequest<Response>;

public class RegisterPcbCommandHandler : IRequestHandler<RegisterPcbCommand, Response>
{
    public Task<Response> Handle(RegisterPcbCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
            register_pcb = new Response.RegisterPcb
            {
                Ipv4Flag = true,
                NextMaintenanceStartAt = 2005364002,
                NextMaintenanceEndAt = 2005364004,
                SramClear = true,
                LmIpAddresses = {"192.168.1.27", "26.30.17.2", "26.30.17.29"},
                ServerInfoes =
                {
                    new Response.RegisterPcb.ServerInfo
                    {
                        ServerType = ServerType.SrvMatch,
                        Uri = $"{command.BaseAddress}/match",
                        Port = 12345
                    },
                    new Response.RegisterPcb.ServerInfo
                    {
                        ServerType = ServerType.SrvStun,
                        Uri = $"{command.BaseAddress}/match",
                        Port = 12345
                    },
                    new Response.RegisterPcb.ServerInfo
                    {
                        ServerType = ServerType.SrvTurn,
                        Uri = $"{command.BaseAddress}/match",
                        Port = 12345
                    }
                },
                core_dump_res =
                {
                    new Response.RegisterPcb.CoreDumpRes
                    {
                        FileName = "test",
                        Url = "http://vsapi.taiko-p.jp/test"
                    }
                }
            }
        };

        return Task.FromResult(response);
    }
}