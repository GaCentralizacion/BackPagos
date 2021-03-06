//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Business.WebReference {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WS_Gen_PdfSoap", Namespace="http://tempuri.org/")]
    public partial class WS_Gen_Pdf : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GeneraPdfPolizaCompraOperationCompleted;
        
        private System.Threading.SendOrPostCallback GenerarPdfArrayOperationCompleted;
        
        private System.Threading.SendOrPostCallback RutaDocumentoOperationCompleted;
        
        private System.Threading.SendOrPostCallback GenerarPdfOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WS_Gen_Pdf() {
            this.Url = global::Business.Properties.Settings.Default.Business_WebReference_WS_Gen_Pdf;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GeneraPdfPolizaCompraCompletedEventHandler GeneraPdfPolizaCompraCompleted;
        
        /// <remarks/>
        public event GenerarPdfArrayCompletedEventHandler GenerarPdfArrayCompleted;
        
        /// <remarks/>
        public event RutaDocumentoCompletedEventHandler RutaDocumentoCompleted;
        
        /// <remarks/>
        public event GenerarPdfCompletedEventHandler GenerarPdfCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GeneraPdfPolizaCompra", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] GeneraPdfPolizaCompra(string PolTipo, string PolAnio, string PolMes, string PolNo, string Empresa) {
            object[] results = this.Invoke("GeneraPdfPolizaCompra", new object[] {
                        PolTipo,
                        PolAnio,
                        PolMes,
                        PolNo,
                        Empresa});
            return ((byte[])(results[0]));
        }
        
        /// <remarks/>
        public void GeneraPdfPolizaCompraAsync(string PolTipo, string PolAnio, string PolMes, string PolNo, string Empresa) {
            this.GeneraPdfPolizaCompraAsync(PolTipo, PolAnio, PolMes, PolNo, Empresa, null);
        }
        
        /// <remarks/>
        public void GeneraPdfPolizaCompraAsync(string PolTipo, string PolAnio, string PolMes, string PolNo, string Empresa, object userState) {
            if ((this.GeneraPdfPolizaCompraOperationCompleted == null)) {
                this.GeneraPdfPolizaCompraOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGeneraPdfPolizaCompraOperationCompleted);
            }
            this.InvokeAsync("GeneraPdfPolizaCompra", new object[] {
                        PolTipo,
                        PolAnio,
                        PolMes,
                        PolNo,
                        Empresa}, this.GeneraPdfPolizaCompraOperationCompleted, userState);
        }
        
        private void OnGeneraPdfPolizaCompraOperationCompleted(object arg) {
            if ((this.GeneraPdfPolizaCompraCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GeneraPdfPolizaCompraCompleted(this, new GeneraPdfPolizaCompraCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GenerarPdfArray", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public byte[][] GenerarPdfArray(string Tipo, string Documento, string Nodo) {
            object[] results = this.Invoke("GenerarPdfArray", new object[] {
                        Tipo,
                        Documento,
                        Nodo});
            return ((byte[][])(results[0]));
        }
        
        /// <remarks/>
        public void GenerarPdfArrayAsync(string Tipo, string Documento, string Nodo) {
            this.GenerarPdfArrayAsync(Tipo, Documento, Nodo, null);
        }
        
        /// <remarks/>
        public void GenerarPdfArrayAsync(string Tipo, string Documento, string Nodo, object userState) {
            if ((this.GenerarPdfArrayOperationCompleted == null)) {
                this.GenerarPdfArrayOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGenerarPdfArrayOperationCompleted);
            }
            this.InvokeAsync("GenerarPdfArray", new object[] {
                        Tipo,
                        Documento,
                        Nodo}, this.GenerarPdfArrayOperationCompleted, userState);
        }
        
        private void OnGenerarPdfArrayOperationCompleted(object arg) {
            if ((this.GenerarPdfArrayCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GenerarPdfArrayCompleted(this, new GenerarPdfArrayCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/RutaDocumento", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string RutaDocumento(string Tipo, string Documento, string Nodo, string Estatus) {
            object[] results = this.Invoke("RutaDocumento", new object[] {
                        Tipo,
                        Documento,
                        Nodo,
                        Estatus});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void RutaDocumentoAsync(string Tipo, string Documento, string Nodo, string Estatus) {
            this.RutaDocumentoAsync(Tipo, Documento, Nodo, Estatus, null);
        }
        
        /// <remarks/>
        public void RutaDocumentoAsync(string Tipo, string Documento, string Nodo, string Estatus, object userState) {
            if ((this.RutaDocumentoOperationCompleted == null)) {
                this.RutaDocumentoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRutaDocumentoOperationCompleted);
            }
            this.InvokeAsync("RutaDocumento", new object[] {
                        Tipo,
                        Documento,
                        Nodo,
                        Estatus}, this.RutaDocumentoOperationCompleted, userState);
        }
        
        private void OnRutaDocumentoOperationCompleted(object arg) {
            if ((this.RutaDocumentoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RutaDocumentoCompleted(this, new RutaDocumentoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GenerarPdf", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] GenerarPdf(string Tipo, string Documento, string Nodo) {
            object[] results = this.Invoke("GenerarPdf", new object[] {
                        Tipo,
                        Documento,
                        Nodo});
            return ((byte[])(results[0]));
        }
        
        /// <remarks/>
        public void GenerarPdfAsync(string Tipo, string Documento, string Nodo) {
            this.GenerarPdfAsync(Tipo, Documento, Nodo, null);
        }
        
        /// <remarks/>
        public void GenerarPdfAsync(string Tipo, string Documento, string Nodo, object userState) {
            if ((this.GenerarPdfOperationCompleted == null)) {
                this.GenerarPdfOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGenerarPdfOperationCompleted);
            }
            this.InvokeAsync("GenerarPdf", new object[] {
                        Tipo,
                        Documento,
                        Nodo}, this.GenerarPdfOperationCompleted, userState);
        }
        
        private void OnGenerarPdfOperationCompleted(object arg) {
            if ((this.GenerarPdfCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GenerarPdfCompleted(this, new GenerarPdfCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void GeneraPdfPolizaCompraCompletedEventHandler(object sender, GeneraPdfPolizaCompraCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GeneraPdfPolizaCompraCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GeneraPdfPolizaCompraCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public byte[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void GenerarPdfArrayCompletedEventHandler(object sender, GenerarPdfArrayCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GenerarPdfArrayCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GenerarPdfArrayCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public byte[][] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((byte[][])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void RutaDocumentoCompletedEventHandler(object sender, RutaDocumentoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RutaDocumentoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RutaDocumentoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void GenerarPdfCompletedEventHandler(object sender, GenerarPdfCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GenerarPdfCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GenerarPdfCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public byte[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591