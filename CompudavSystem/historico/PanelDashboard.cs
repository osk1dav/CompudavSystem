﻿using CompudavSystem.bdd;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CompudavSystem.historico
{
    public partial class PanelDashboard : Form
    {
        public PanelDashboard()
        {
            InitializeComponent();
            DiaActual();
            rangoFechaComboBox.SelectedIndex = 2;
            DatosIniciales();
        }
        private void DiaActual()
        {
            fechaActualTextBox.Text = DateTime.Today.Day.ToString();
        }

        public void DatosIniciales()
        {
            DateTime dateTimeStart = toDateTimePicker.Value;
            string fechaInicio = $"'{dateTimeStart: yyyy-MM-dd} 00:00:00'";
            DateTime dateTimeEnd = fromDateTimePicker.Value;
            string fechaFin = $"'{dateTimeEnd: yyyy-MM-dd} 23:59:59'";
            listadoDataGridView.DataSource = ConsultasSql.TopItems("document_history", "product", "quantity", "total_value", "type_document", "VENTA", "status_document", "Autorizado", "date_of_issue", fechaInicio, fechaFin,"5");
            listadoDataGridView.Columns["product"].Width = 318;
            listadoDataGridView.Columns["product"].DefaultCellStyle.Padding = new Padding(15, 0, 0, 0);
            listadoDataGridView.Columns["quantity"].Width = 60;
            listadoDataGridView.Columns["quantity"].DefaultCellStyle.Format = "N0";
            listadoDataGridView.Columns["quantity"].DefaultCellStyle.Padding = new Padding(0, 0, 15, 0);
            listadoDataGridView.Columns["quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            listadoDataGridView.Columns["total_value"].Width = 120;
            listadoDataGridView.Columns["total_value"].DefaultCellStyle.Format = "C2";
            listadoDataGridView.Columns["total_value"].DefaultCellStyle.Padding = new Padding(0, 0, 15, 0);
            listadoDataGridView.Columns["total_value"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            listadoDataGridView.Columns["date_of_issue"].Visible = false;

            listadoDataGridView.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#E5E5E5");
            listadoDataGridView.DefaultCellStyle.SelectionForeColor = ColorTranslator.FromHtml("#000000");

            DataTable dataValorTotal = ConsultasSql.SumaTotalItem("document_history", "total_value", "type_document", "VENTA", "status_document", "Autorizado", "date_of_issue", fechaInicio, fechaFin);
            decimal.TryParse(dataValorTotal.Rows[0][0].ToString(), out decimal totalVentasDecimal);
            totalVentasTextBox.Text = Math.Round(totalVentasDecimal, 2).ToString("C2");
            
            DataTable dataTotalDocumentos = ConsultasSql.ConteoTotalItem("document_history", "type_document", "VENTA", "status_document", "Autorizado", "date_of_issue", fechaInicio, fechaFin);
            int.TryParse(dataTotalDocumentos.Rows[0][0].ToString(), out int totalFacturas);
            if (totalFacturas == 0)
            {
                totalFacturasTextBox.Text = "No hay facturas registradas";
            }
            else if (totalFacturas >= 1)
            {
                totalFacturasTextBox.Text = $"Facturas totales : {dataTotalDocumentos.Rows[0][0]}";
            }

            stockDataGridView.DataSource = ConsultasSql.ConsultaGeneral("product", "name,stock, minimum_stock_level");

        }

        private void RangoFechaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (rangoFechaComboBox.SelectedIndex)
            {
                case 0: //Hoy
                    toDateTimePicker.Value = DateTime.Today;
                    fromDateTimePicker.Value = DateTime.Today;
                    toDateTimePicker.Enabled = false;
                    fromDateTimePicker.Enabled = false;
                    break;
                case 1: //Ayer
                    toDateTimePicker.Value = DateTime.Today.AddDays(-1);
                    fromDateTimePicker.Value = DateTime.Today.AddDays(-1);
                    toDateTimePicker.Enabled = false;
                    fromDateTimePicker.Enabled = false;
                    break;
                case 2: //Este mes
                    toDateTimePicker.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    fromDateTimePicker.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1);
                    toDateTimePicker.Enabled = false;
                    fromDateTimePicker.Enabled = false;
                    break;
                case 3: //Mes anterior
                    toDateTimePicker.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
                    fromDateTimePicker.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);
                    toDateTimePicker.Enabled = false;
                    fromDateTimePicker.Enabled = false;
                    break;
                case 4: //Últimos 30 días
                    toDateTimePicker.Value = DateTime.Today.AddMonths(-1);
                    fromDateTimePicker.Value = DateTime.Today;
                    toDateTimePicker.Enabled = false;
                    fromDateTimePicker.Enabled = false;
                    break;
                case 5: //Último trimestre
                    toDateTimePicker.Value = DateTime.Today.AddMonths(-3);
                    fromDateTimePicker.Value = DateTime.Today;
                    toDateTimePicker.Enabled = false;
                    fromDateTimePicker.Enabled = false;
                    break;
                case 6: //Último semestre
                    toDateTimePicker.Value = DateTime.Today.AddMonths(-6);
                    fromDateTimePicker.Value = DateTime.Today;
                    toDateTimePicker.Enabled = false;
                    fromDateTimePicker.Enabled = false;
                    break;
                case 7: //Este año
                    toDateTimePicker.Value = new DateTime(DateTime.Today.Year, 1, 1);
                    fromDateTimePicker.Value = new DateTime(DateTime.Today.Year, 12, 1).AddMonths(1).AddDays(-1);
                    toDateTimePicker.Enabled = false;
                    fromDateTimePicker.Enabled = false;
                    break;
                case 8: //Año anterior
                    toDateTimePicker.Value = new DateTime(DateTime.Today.Year, 1, 1).AddYears(-1);
                    fromDateTimePicker.Value = new DateTime(DateTime.Today.Year, 12, 1).AddYears(-1).AddMonths(1).AddDays(-1);
                    toDateTimePicker.Enabled = false;
                    fromDateTimePicker.Enabled = false;
                    break;
                case 9: //Seleccionar rango
                    toDateTimePicker.Enabled = true;
                    fromDateTimePicker.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        private void ToDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (toDateTimePicker.Value.Ticks > fromDateTimePicker.Value.Ticks)
            {
                toDateTimePicker.Value = fromDateTimePicker.Value;
            }
            if (toDateTimePicker.Value.Ticks <= fromDateTimePicker.Value.Ticks)
            {
                DatosIniciales();
            }
        }

        private void FromDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (fromDateTimePicker.Value.Ticks < toDateTimePicker.Value.Ticks)
            {
                fromDateTimePicker.Value = toDateTimePicker.Value;
            }
            if (fromDateTimePicker.Value.Ticks >= toDateTimePicker.Value.Ticks)
            {
                DatosIniciales();
            }
        }

        private void PanelDashboard_Load(object sender, EventArgs e)
        {
            listadoDataGridView.ClearSelection();
        }

        private void listadoDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}