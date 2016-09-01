<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SisRptBanco.UI.Default" %>

<%@ Register Assembly="DevExpress.Web.v15.2, Version=15.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="vendors/bootstrap-3.3.6-dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="vendors/jquery-ui-1.11.4/jquery-ui.min.css" rel="stylesheet" />
    <link href="resources/css/Default.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="container">
                <div class="page-header">
                    <h1>Movimientos Bancarios
                        <small>Control de ingreso y salida</small>
                    </h1>
                </div>
                <dx:ASPxComboBox ID="cbTemas" runat="server" AutoPostBack="True" CssClass="cbThemeSelector">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
	ASPxClientUtils.SetCookie('theme', s.GetValue());
}" />
                    </dx:ASPxComboBox>
                <div class="lblThemeSelector">
                    <p>Temas</p>
                </div>
                <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" ColCount="3">
                    <Items>
                        <dx:LayoutItem Caption="Inicio">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxDateEdit ID="deFechaInicio" runat="server">
                                    </dx:ASPxDateEdit>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Fin">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxDateEdit ID="deFechaFin" runat="server">
                                    </dx:ASPxDateEdit>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Cta. Bancaria" ColSpan="2">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxGridLookup ID="gluCuentasBancarias" runat="server" AutoGenerateColumns="False" DataSourceID="edsCuentasBancarias" KeyFieldName="IdCtaxBco" Width="100%">
                                        <GridViewProperties>
                                            <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                            <Settings ShowFilterRow="True" />
                                        </GridViewProperties>
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="cDesBco" VisibleIndex="1" Caption="Banco">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="cNroCta" VisibleIndex="2" Caption="Nro de Cuenta">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewCommandColumn ShowClearFilterButton="True" VisibleIndex="0">
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataTextColumn Caption="Institución" FieldName="TTipIns.cDesIns" ShowInCustomizationForm="True" VisibleIndex="3">
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:ASPxGridLookup>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxButton ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Text="Buscar">
                                        <Image IconID="find_find_16x16">
                                        </Image>
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Cuenta Actual:" ColSpan="3">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxLabel ID="lblCuentaActual" runat="server">
                                    </dx:ASPxLabel>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                            <CaptionStyle Font-Bold="True">
                            </CaptionStyle>
                            <ParentContainerStyle>
                                <Border BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px" />
                            </ParentContainerStyle>
                        </dx:LayoutItem>
                    </Items>
                </dx:ASPxFormLayout>
                <dx:ASPxGridView ID="gvMovPorCuenta" runat="server" AutoGenerateColumns="False" DataSourceID="edsMovPorCuenta" KeyFieldName="IdMovCtaxBco" ClientInstanceName="gvMovPorCuenta" OnCommandButtonInitialize="gvMovPorCuenta_CommandButtonInitialize" OnInitNewRow="gvMovPorCuenta_InitNewRow">
                    <Settings ShowFilterRow="True" ShowFilterRowMenu="True" />
                    <SettingsBehavior AllowSort="False" />
                    <SettingsCommandButton>
                        <NewButton ButtonType="Button">
                            <Image IconID="actions_addfile_16x16">
                            </Image>
                        </NewButton>
                        <UpdateButton ButtonType="Button" Text="Guardar">
                            <Image IconID="actions_apply_16x16">
                            </Image>
                        </UpdateButton>
                        <CancelButton ButtonType="Button">
                            <Image IconID="actions_cancel_16x16">
                            </Image>
                        </CancelButton>
                        <EditButton ButtonType="Image">
                            <Image IconID="actions_editname_16x16">
                            </Image>
                        </EditButton>
                        <DeleteButton ButtonType="Image">
                            <Image IconID="actions_clear_16x16">
                            </Image>
                        </DeleteButton>
                    </SettingsCommandButton>
                    <Columns>
                        <dx:GridViewCommandColumn ShowDeleteButton="True" ShowEditButton="True" VisibleIndex="0" ButtonType="Image" ShowNewButtonInHeader="True">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataDateColumn FieldName="dFecOpe" VisibleIndex="1" Caption="Fecha de Operación">
                            <PropertiesDateEdit DisplayFormatString="G" EditFormat="DateTime" EditFormatString="dd/MM/yyyy hh:mm:ss tt">
                                <ValidationSettings>
                                    <RequiredField ErrorText="Campo Requerido" IsRequired="True" />
                                </ValidationSettings>
                            </PropertiesDateEdit>
                        </dx:GridViewDataDateColumn>
                        <dx:GridViewDataTextColumn FieldName="cNroOpe" VisibleIndex="2" Caption="Nro de Operación">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="cNroFac" VisibleIndex="6" Caption="Nro de Factura">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataSpinEditColumn Caption="Importe" FieldName="nImporte" VisibleIndex="4">
                            <PropertiesSpinEdit DecimalPlaces="4" DisplayFormatString="c" NumberFormat="Currency">
                                <ValidationSettings>
                                    <RequiredField ErrorText="Campo Requerido" IsRequired="True" />
                                </ValidationSettings>
                            </PropertiesSpinEdit>
                        </dx:GridViewDataSpinEditColumn>
                        <dx:GridViewDataMemoColumn Caption="Concepto" FieldName="cConcepto" VisibleIndex="3">
                            <PropertiesMemoEdit MaxLength="200" Rows="4">
                                <ValidationSettings>
                                    <RequiredField ErrorText="Campo Requerido" IsRequired="True" />
                                </ValidationSettings>
                            </PropertiesMemoEdit>
                        </dx:GridViewDataMemoColumn>
                        <dx:GridViewDataMemoColumn Caption="Destino" FieldName="cDestino" VisibleIndex="5">
                            <PropertiesMemoEdit MaxLength="300" Rows="4">
                            </PropertiesMemoEdit>
                        </dx:GridViewDataMemoColumn>
                        <dx:GridViewDataSpinEditColumn Caption="Saldo Final" FieldName="nSalFin" ReadOnly="True" VisibleIndex="7">
                            <PropertiesSpinEdit DisplayFormatString="c" NumberFormat="Currency">
                            </PropertiesSpinEdit>
                            <EditFormSettings Visible="False" />
                        </dx:GridViewDataSpinEditColumn>
                    </Columns>
                </dx:ASPxGridView>
                <asp:EntityDataSource ID="edsCuentasBancarias" runat="server" ConnectionString="name=LaPePeEntities" DefaultContainerName="LaPePeEntities" EnableFlattening="False" EntitySetName="MCtaxBco" OnSelecting="edsCuentasBancarias_Selecting">
                </asp:EntityDataSource>
                <asp:EntityDataSource ID="edsMovPorCuenta" runat="server" ConnectionString="name=LaPePeEntities" DefaultContainerName="LaPePeEntities" EnableDelete="True" EnableFlattening="False" EnableInsert="True" EnableUpdate="True" EntitySetName="DMovCtaxBco" OnSelecting="edsMovPorCuenta_Selecting" Where="" OnInserting="edsMovPorCuenta_Inserting" EntityTypeFilter="" Select="" OnDeleting="edsMovPorCuenta_Deleting" OnUpdating="edsMovPorCuenta_Updating">
                </asp:EntityDataSource>

                <script src="vendors/jquery-2.2.4.min.js"></script>
                <script src="vendors/jquery-ui-1.11.4/jquery-ui.min.js"></script>
                <script src="vendors/bootstrap-3.3.6-dist/js/bootstrap.min.js"></script>
            </div>
        </div>
    </form>
</body>
</html>
