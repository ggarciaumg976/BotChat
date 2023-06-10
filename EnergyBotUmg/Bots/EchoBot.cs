// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.18.1

using EnergyBotUmg.Entidades;
using EnergyBotUmg.Servicio;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EnergyBotUmg.Bots
{
    public class EchoBot : ActivityHandler
    {
        private readonly IConfiguration config;
        private readonly IServicioConsulta consul;
        public string estado;
        public EchoBot(IConfiguration config, IServicioConsulta consul)
        {
            this.config = config;
            this.consul = consul;
            estado = "0";
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var replyText = $"{turnContext.Activity.Text}";
            string concorr = null;

            if (estado != "0")
            {
                estado = $"{estado}.{replyText}";
            }
            else
            {
                estado = $"{replyText}";
            }

            if(estado.Contains("1.1.1"))
            {
                concorr = replyText;
                estado = "1.1.1";
            }

            if (estado.Contains("1.2.1"))
            {
                concorr = replyText;
                estado = "1.2.1";
            }

            if (estado.Contains("2.1.1"))
            {
                concorr = replyText;
                estado = "2.1.1";
            }

            LogTransaccion log = new LogTransaccion();
            switch (estado)
            {
                case "1":
                    replyText = $"Consulta de saldo  " + System.Environment.NewLine
                        + "1. Para Consulta por Contador " + System.Environment.NewLine
                        + "2. Para Consulta por Correlativo ";
                    await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                    break;
                case "1.1":
                    replyText = $"Ingresa Numero de Contador ";
                    await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                    estado = "1.1.1";
                    break;
                case "1.1.1":
                    var resultContador = await consul.ClienteSaldoContador(concorr);
                    if(resultContador != null)
                    {
                        replyText = $"Contador a Nombre de : {resultContador.nombreCliente} Su saldo es : {resultContador.Saldo}" + Environment.NewLine +
                            "quieres regresar al menu principal? Si/No ";
                        estado = "0";
                        log.tipo = "ConsultaContador";
                        log.idConsulta = concorr;
                        var resultInsertCon = await consul.inslogTransaccion(log);
                        await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                    }
                    else
                    {
                        replyText = $"El numero de contador ingresado no existe " + Environment.NewLine +
                            "quieres regresar al menu principal? Si/No ";
                        await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                        estado = "0";
                    }
                    break;
                case "1.2":
                    replyText = $"Ingresa Numero de Correlativo ";
                    await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                    estado = "1.2.1";
                    break;
                case "1.2.1":
                    var resultCorrelativo = await consul.ClienteSaldoCorrelativo(concorr);
                    if (resultCorrelativo != null)
                    {
                        replyText = $"Correlativo a Nombre de : {resultCorrelativo.nombreCliente} Su saldo es : {resultCorrelativo.Saldo}" + Environment.NewLine +
                            "quieres regresar al menu principal? Si/No ";
                        estado = "0";
                        log.tipo = "ConsultaCorrelativo";
                        log.idConsulta = concorr;
                        var resultInsertCor = await consul.inslogTransaccion(log);
                        await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                    }
                    else 
                    {
                        replyText = $"El Numero de Correlativo ingresado no existe " + Environment.NewLine +
                            "quieres regresar al menu principal? Si/No ";
                        await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                        estado = "0";
                    }
                    break;
                case "2":
                    replyText = $"Pago en Linea  " + Environment.NewLine +
                        $"Ingresa Numero de Contador ";
                    await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                    estado = "2.1.1";
                    break;
                case "2.1.1":
                    var resultPagoContador = await consul.ClienteSaldoContador(concorr);
                    if(resultPagoContador != null)
                    {
                        replyText = $"https://localhost:44356/PagoEnLinea/{resultPagoContador.Saldo}" + Environment.NewLine +
                            "quieres regresar al menu principal? Si/No ";
                        estado = "0";
                        await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                    }
                    else
                    {
                        replyText = $"El Numero de contador ingresado no existe o no tiene saldo pendiente " + Environment.NewLine +
                            "quieres regresar al menu principal? Si/No ";
                        await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                        estado = "0";
                    }
                    break;
                case "3":
                    replyText = $"Para ingreso de reclamo ve a este sitio https://localhost:44356/reclamo" + Environment.NewLine +
                        "quieres regresar al menu principal? Si/No ";
                    estado = "0";
                    await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                    break;
                case "9":
                case "Si":
                case "si":
                case "SI":
                    replyText = " Elige: " + Environment.NewLine +
                                " 1. Para consultar saldos " + Environment.NewLine +
                                " 2. Para pago de servicio " + Environment.NewLine +
                                " 3. Ingreso de Reclamo ";
                    await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                    estado = "0";
                    break;
                case "No":
                case "no":
                case "NO":
                case "Cancelar":
                case "Salir":
                    replyText = "Fue un placer ayudarte ";
                    await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                    estado = "0";
                    break;
            }
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hola soy Energy la asistente virtual de Empresa Electrica (*Un Bot*), " + Environment.NewLine +
                " Elige: " + Environment.NewLine +
                " 1. Para consultar saldos " + Environment.NewLine +
                " 2. Para pago de servicio " + Environment.NewLine +
                " 3. Ingreso de Reclamo " + Environment.NewLine ;
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
