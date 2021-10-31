﻿using ApacheTech.VintageMods.Core.Abstractions.ModSystems.Composite;
using ApacheTech.VintageMods.Core.Hosting;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;
using ApacheTech.VintageMods.FluentChatCommands;
using Microsoft.Extensions.DependencyInjection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace ApacheTech.VintageMods.WaypointExtensions
{
    /// <summary>
    ///     Entry-point for the mod. This class will configure and build the IOC Container, and Service list for the rest of the mod.
    ///     
    ///     Registrations performed within this class should be global scope; by convention, features should aim to be as stand-alone as they can be.
    /// </summary>
    /// <seealso cref="ModHost" />
    internal sealed class Program : ModHost
    {
        /// <summary>
        ///     Allows a mod to include Singleton, Scoped, or Transient services to the DI Container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        protected override void ConfigureServerModServices(IServiceCollection services)
        {
            services.AddGlobalConfiguration("settings-global-server.json");
            services.AddWorldConfiguration("settings-world-server.json");
        }

        /// <summary>
        ///     Allows a mod to include Singleton, Scoped, or Transient services to the DI Container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        protected override void ConfigureClientModServices(IServiceCollection services)
        {
            services.AddGlobalConfiguration("settings-global-client.json");
            services.AddWorldConfiguration("settings-world-client.json");
        }

        /// <summary>
        ///     Called on the client, during initial mod loading, called before any mod receives the call to Start().
        /// </summary>
        /// <param name="capi">
        ///     The core API implemented by the client.
        ///     The main interface for accessing the client.
        ///     Contains all sub-components, and some miscellaneous methods.
        /// </param>
        protected override void StartPreClientSide(ICoreClientAPI capi)
        {
            ModServices.FileSystem
                .RegisterFile("version-installed.json", FileScope.Global);
        }

        /// <summary>
        ///     Called on the server, during initial mod loading, called before any mod receives the call to Start().
        /// </summary>
        /// <param name="sapi">
        ///     The core API implemented by the server.
        ///     The main interface for accessing the server.
        ///     Contains all sub-components, and some miscellaneous methods.
        /// </param>
        protected override void StartPreServerSide(ICoreServerAPI sapi)
        {
            ModServices.FileSystem
                .RegisterFile("version-installed.json", FileScope.Global);

            FluentChat
                .ServerCommand("wpconfig")
                .HasDescription(Lang.Get("wpex:features.wpconfig.server.description"))
                .RequiresPrivilege(Privilege.controlserver)
                .RegisterWith(sapi);
        }

        /// <summary>
        ///     Side agnostic Start method, called after all mods received a call to StartPre().
        /// </summary>
        /// <param name="api">The API.</param>
        public override void Start(ICoreAPI api)
        {
            ModServices.Harmony.UseHarmony();
        }
    }
}