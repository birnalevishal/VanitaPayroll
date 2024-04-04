<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PayRoll._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="content">

                <div class="row">
                    <div class="col-lg-12 text-center welcome-message">
                        <h3>Welcome to <strong><%: Session["OrgName"].ToString() %> </strong>Organisation
                        </h3>
                    </div>
                </div>

                <div class="row">
                    <div class="col-lg-12">
                        <div class="hpanel ">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <%--<a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>--%>
                                </div>
                                Dashboard information
                            </div>
                            <div class="panel-body">
                            </div>
                            <div class="panel-footer">
                                <span class="pull-right"></span>
                                Last update: <%: DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss") %>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row col-lg-6">

                    <div class="row col-lg-12" id="divPerformace">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Perfomance Evaluation List 
                            </div>
                            <div class="panel-body list">
                                <div class="table-responsive project-list">
                                    <asp:GridView ID="gvPerEvalList" runat="server" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="5" Width="100%" OnPageIndexChanging="gvList_PageIndexChanging" TabIndex="6">
                                        <Columns>
                                            <asp:BoundField DataField="Employeecd" HeaderText="Emp Code" HeaderStyle-CssClass="col-lg-2">
                                                <HeaderStyle CssClass="col-lg-2" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Employeename" HeaderText="Name" HeaderStyle-CssClass="col-lg-4">
                                                <HeaderStyle CssClass="col-lg-6" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OrigJoindate" HeaderText="Join date" HeaderStyle-CssClass="col-lg-2" DataFormatString="{0:d}">
                                                <HeaderStyle CssClass="col-lg-2" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Period" HeaderText="Period" HeaderStyle-CssClass="col-lg-2">
                                                <HeaderStyle CssClass="col-lg-2" />
                                            </asp:BoundField>

                                        </Columns>
                                        <PagerStyle CssClass="GridPager" />
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="panel-footer">
                                <span class="pull-right"></span>
                                <asp:LinkButton ID="lbtnRpt" runat="server" OnClick="lbtnRpt_Click"> Click here for Report</asp:LinkButton>

                            </div>
                        </div>
                    </div>

                    <div class="row col-lg-12" id="divRetiring">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Retiring Employee List 
                            </div>
                            <div class="panel-body list">
                                <div class="table-responsive project-list">
                                    <asp:GridView ID="gvRetiringEmpList" runat="server" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="5" Width="100%" TabIndex="6" OnPageIndexChanging="gvRetiringEmpList_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="Employeecd" HeaderText="Emp Code" HeaderStyle-CssClass="col-lg-2">
                                                <HeaderStyle CssClass="col-lg-2" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Employeename" HeaderText="Name" HeaderStyle-CssClass="col-lg-4">
                                                <HeaderStyle CssClass="col-lg-6" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Birthdate" HeaderText="Birth Date" HeaderStyle-CssClass="col-lg-2" DataFormatString="{0:d}">
                                                <HeaderStyle CssClass="col-lg-2" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DaysLeft" HeaderText="Left Days" HeaderStyle-CssClass="col-lg-2">
                                                <HeaderStyle CssClass="col-lg-2" />
                                            </asp:BoundField>

                                        </Columns>
                                        <PagerStyle CssClass="GridPager" />
                                    </asp:GridView>
                                </div>
                            </div>
                            <%--<div class="panel-footer">
                                <span class="pull-right"></span>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lbtnRpt_Click"> Click here for Report</asp:LinkButton>
                               
                            </div>--%>
                        </div>
                    </div>

                </div>

                <div class="row col-lg-6">
                    <div class="row col-lg-12" id="divLeftEmployeePFList">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Left Employee PF Withdrwad Intimation List 
                            </div>
                            <div class="panel-body list">
                                <div class="table-responsive project-list">
                                    <asp:GridView ID="gvLeftEmployeeList" runat="server" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="5" Width="100%" TabIndex="6" OnPageIndexChanging="gvLeftEmployeeList_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="Employeecd" HeaderText="Emp Code" HeaderStyle-CssClass="col-lg-2">
                                                <HeaderStyle CssClass="col-lg-2" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Employeename" HeaderText="Name" HeaderStyle-CssClass="col-lg-4">
                                                <HeaderStyle CssClass="col-lg-6" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Leavedate" HeaderText="Leave Date" HeaderStyle-CssClass="col-lg-2" DataFormatString="{0:d}">
                                                <HeaderStyle CssClass="col-lg-2" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DaysFromLeftDate" HeaderText="Left Days" HeaderStyle-CssClass="col-lg-2">
                                                <HeaderStyle CssClass="col-lg-2" />
                                            </asp:BoundField>

                                        </Columns>
                                        <PagerStyle CssClass="GridPager" />
                                    </asp:GridView>
                                </div>
                            </div>
                            <%--<div class="panel-footer">
                                <span class="pull-right"></span>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lbtnRpt_Click"> Click here for Report</asp:LinkButton>
                               
                            </div>--%>
                        </div>
                    </div>

                    <div class="row col-lg-6">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Calender
                            </div>
                            <div class="panel-body list">
                                <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="#3366CC" BorderWidth="1px" Font-Names="Verdana" Font-Size="8pt" ForeColor="#003399" Height="200px" Width="210px" CellPadding="1" DayNameFormat="Shortest" FirstDayOfWeek="Monday">
                                    <DayHeaderStyle BackColor="#99CCCC" ForeColor="#336666" Height="1px" />
                                    <NextPrevStyle Font-Size="8pt" ForeColor="#CCCCFF" />
                                    <OtherMonthDayStyle ForeColor="#999999" />
                                    <SelectedDayStyle BackColor="#009999" ForeColor="#CCFF99" Font-Bold="True" />
                                    <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                                    <TitleStyle BackColor="#003399" BorderColor="#3366CC" BorderWidth="1px" Font-Bold="True" Font-Size="10pt" ForeColor="#CCCCFF" Height="25px" />
                                    <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
                                    <WeekendDayStyle BackColor="#CCCCFF" />
                                </asp:Calendar>
                            </div>
                            <div class="panel-footer">
                                <span class="pull-right"></span>
                                <asp:LinkButton ID="lbtnDBBackup" runat="server" OnClick="lbtnDBBackup_Click"> Click here for Database Backup</asp:LinkButton>

                            </div>
                        </div>
                    </div>

                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
