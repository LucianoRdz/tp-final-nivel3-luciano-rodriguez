﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using negocio;
using dominio;
using System.Data.SqlTypes;

namespace Tienda_Electronica_Web
{
    public partial class FormAlta : System.Web.UI.Page
    {
        public bool ConfirmaEliminacion { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            txtId.Enabled = false;
            ConfirmaEliminacion = false;
            try
            {
                //Modificacion inicial
                if (!IsPostBack)
                {
                    CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
                    MarcaNegocio marcaNegocio = new MarcaNegocio();
                    List<Categoria> listaCategorias = categoriaNegocio.listar();
                    List<Marca> listaMarcas = marcaNegocio.listar();

                    ddlMarca.DataSource = listaMarcas;
                    ddlMarca.DataValueField = "Id";
                    ddlMarca.DataTextField = "Descripcion";
                    ddlMarca.DataBind();                    

                    ddlCategoria.DataSource = listaCategorias;
                    ddlCategoria.DataValueField = "Id";
                    ddlCategoria.DataTextField = "Descripcion";
                    ddlCategoria.DataBind();
                }

                // configuracion si se esta modificando

                string id = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";
                if (id != "" && !IsPostBack)
                {
                    ArticuloNegocio negocio = new ArticuloNegocio();
                    Articulo seleccionado = (negocio.listar(id))[0];
                    //pre cargar los campos
                    txtId.Text = id;
                    txtNombre.Text = seleccionado.Nombre;
                    txtPrecio.Text = seleccionado.Precio.ToString();
                    txtCodigo.Text = seleccionado.Codigo;
                    txtDescripcion.Text = seleccionado.Descripcion;
                    txtImagenUrl.Text = seleccionado.ImagenUrl;
                    imgArticulo.ImageUrl = seleccionado.ImagenUrl;

                    ddlCategoria.SelectedValue = seleccionado.Categoria.Id.ToString();
                    ddlMarca.SelectedValue = seleccionado.Marca.Id.ToString();
                }

            }
            catch (Exception ex)
            {
                Session.Add("error", ex);
                Response.Redirect("Error.aspx", false);
               
            }

        }

        protected void txtImagenUrl_TextChanged(object sender, EventArgs e)
        {
            try
            {
                imgArticulo.ImageUrl = txtImagenUrl.Text;
            }
            catch (Exception ex)
            {

                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx", false);
            }
            
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {


                try
                {
                    Page.Validate();
                    if (!Page.IsValid)
                        return;

                    Articulo nuevo = new Articulo();
                    ArticuloNegocio negocio = new ArticuloNegocio();

                    nuevo.Nombre = txtNombre.Text;
                    nuevo.Codigo = txtCodigo.Text;
                    nuevo.Descripcion = txtDescripcion.Text;
                    nuevo.ImagenUrl = txtImagenUrl.Text;
                    nuevo.Precio = decimal.Parse(txtPrecio.Text);

                    nuevo.Categoria = new Categoria();
                    nuevo.Categoria.Id = int.Parse(ddlCategoria.SelectedValue);

                    nuevo.Marca = new Marca();
                    nuevo.Marca.Id = int.Parse(ddlMarca.SelectedValue);

                    if (Request.QueryString["id"] != null)
                    {
                        nuevo.Id = int.Parse(txtId.Text);
                        negocio.modificar(nuevo);
                    }
                    else
                    {
                        negocio.agregar(nuevo);
                    }
                    Response.Redirect("Lista.aspx", false);
                }
            catch (Exception ex)
            {

                Session.Add("error", ex.ToString()); ;
            }
         }

        
     

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            ConfirmaEliminacion = true;
        }

        protected void btnConfirmaEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkConfirmaEliminacion.Checked)
                {
                ArticuloNegocio negocio = new ArticuloNegocio();
                negocio.eliminar(int.Parse(txtId.Text));
                Response.Redirect("Lista.aspx");
                }
            }
            catch (Exception ex )
            {

                Session.Add("error", ex);
            }
        }
    }
}