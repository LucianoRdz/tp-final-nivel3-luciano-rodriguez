﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dominio;
using negocio;

namespace Tienda_Electronica_Web
{
    public partial class Master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!( Page is Login || Page is Registro))
            {
                if (!Seguridad.sesionActiva(Session["usuario"]))
                    Response.Redirect("Login.aspx", false);
            }
                
        }

        protected void imgAvatar_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("MiPerfil.aspx");
        }
    }
}