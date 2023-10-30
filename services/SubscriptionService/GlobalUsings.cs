// Global using directives

global using System.ComponentModel.DataAnnotations;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using Core.Exceptions;
global using Core.Helpers;
global using Core.Providers;
global using Dapr;
global using Dapr.Client;
global using EventBus;
global using EventBus.Abstractions;
global using HealthChecks;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.Extensions.Options;
global using Microsoft.OpenApi.Models;
global using SubscriptionAPI.Commands;
global using SubscriptionService;
global using SubscriptionService.Events;
global using SubscriptionService.Model;
global using SubscriptionService.Proxy;
global using SubscriptionService.Repository;