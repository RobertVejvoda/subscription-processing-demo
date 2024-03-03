// Global using directives

global using System.ComponentModel.DataAnnotations;
global using System.Text.Json;
global using Camunda.Abstractions;
global using Camunda.Command;
global using Core.Helpers;
global using Core.Providers;
global using Core.Types;
global using Dapr.Client;
global using HealthChecks;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.OpenApi.Models;
global using SubscriptionService.Commands;
global using SubscriptionService.Proxy;
global using SubscriptionService.Repository;
global using Subscription = SubscriptionService.Domain.Subscription;