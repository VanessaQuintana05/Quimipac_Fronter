using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace Quimipac_.Models
{
    public class MailKit_SMTP
    {
        public bool EnviarMail(string EmailEnvio,string NombreContrato, string Mensaje, string mailDominio, string Mail, string Puerto, string SSL, string Usuario, string Clave, string Cuerpo, string EsHtml, string Asunto, string descripcion_tipo_not)
        {
            try
            {
                string CorreoHtml = "<br/><center>"
                               + "<div>"
                               + "<div style='font-family: Roboto-Regular, Helvetica, Arial, sans-serif; padding: 40px 20px 38px; color: rgba(0, 0, 0, 0.87); ' >"
                               + "<table style='text-align: center; min-width: 348px;' width='50%' cellspacing = '0' cellpadding='0' border='0' > "
                                    + "<tbody>"
                                      + "<tr>"
                                        + "<td><span style='font-family: Arial; font-size: 12px; text-align: center;'>Estimad@: Su " + descripcion_tipo_not + ": " + NombreContrato+" recibio la siguiente alerta</span><br><br>"
                                           + "<div style='text-align: center;'><span style='text-align: center; font-size: 12px;'>"
                                            // + "<p>Por favor ingresar como contraseña la siguiente clave temporal. Luego de iniciar sesión proceder a cambiar la contraseña desde el aplicativo.<br> <br><b>{clave}</b> </br></span></p></div>"
                                            + "<p>"+Mensaje+"<br> <br><b></b> </br></span></p></div>"
                                                
                                            //+ WebConfigurationManager.AppSettings["CuerpoMail"].ToString() + 
                                            + "<hr>"
                                          + "<div style='text-align: center;'><span style='font-size: 8pt; color: #c0c0c0;'>" +"</span>"//+ WebConfigurationManager.AppSettings["PieMail"].ToString() + 
                                          + "</div><div style='text-align: center; direction: ltr;'>"
                                       // + "<span style='font-size: 8pt; color: #c0c0c0;'>© Genfix "+DateTime.Now.Year+"</span></div>"
                                       + "</td>"
                                      + "</tr>"
                                   + " </tbody>"
                                  + "</table>"
                                  + "</div>"
                                  + "</div>"
                                  + "</center>";

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(Mail));//("Joey Tribbiani", "@.com"));
                message.To.Add(new MailboxAddress(EmailEnvio));//"g", "@.com"));
                message.Subject = Asunto;

                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = CorreoHtml//@"Demo"
                };

                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    //client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    //client.Connect("smtp.friends.com", 587, false);


                    var bandSSL = (SSL == "1") ? true : false;
                    if (bandSSL)
                    {
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    }
                        
                        client.Connect(mailDominio, int.Parse(Puerto), false);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(Mail, Clave);

                    client.Send(message);
                    client.Disconnect(true);
                }
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e + "||Err: " + e.Message);
                return false;
            }
        }


        
    }
}