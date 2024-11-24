﻿using PBL_LP.Models;
using System.Data.SqlClient;
using System.Data;

namespace PBL_LP.DAO
{
    public class AluguelDAO : PadraoDAO<AluguelViewModel>
    {
        public void Inserir(AluguelViewModel aluguel)
        {
            HelperDAO.ExecutaProc("spInsert_Aluguel", CriaParametros(aluguel));
        }


        public void Alterar(AluguelViewModel aluguel)
        {
            HelperDAO.ExecutaProc("spUpdate_Aluguel", CriaParametros(aluguel));
        }

        protected override SqlParameter[] CriaParametros(AluguelViewModel aluguel)
        {
            SqlParameter[] parametros = new SqlParameter[7];
            parametros[0] = new SqlParameter("codigoDoAluguel", aluguel.CodigoDoAluguel);
            parametros[1] = new SqlParameter("cnpj", aluguel.CNPJ);
            parametros[2] = new SqlParameter("codigoSensor", aluguel.CodigoSensor);
            parametros[3] = new SqlParameter("quantidade", aluguel.Quantidade);
            parametros[4] = new SqlParameter("dataDeInicio", aluguel.DataDeInicio);
            parametros[5] = new SqlParameter("dataDeFinalizacao", aluguel.DataDeFinalizacao);
            parametros[6] = new SqlParameter("preco", aluguel.Preco);
            return parametros;
        }

        public void Excluir(int id)
        {
            var p = new SqlParameter[]
            {
                 new SqlParameter("id", id),
                 new SqlParameter("tabela", "Aluguel")
            };

            HelperDAO.ExecutaProc("spDelete", p);
        }

        protected override AluguelViewModel MontaModel(DataRow registro)
        {
            AluguelViewModel a = new AluguelViewModel
            {
                CodigoDoAluguel = Convert.ToInt32(registro["CodigoDoAluguel"]),
                CNPJ = registro["CNPJ"].ToString(),
                CodigoSensor = Convert.ToInt32(registro["CodigoSensor"]),
                Quantidade = Convert.ToInt32(registro["Quantidade"]),
                DataDeInicio = Convert.ToDateTime(registro["DataDeInicio"]),
                DataDeFinalizacao = Convert.ToDateTime(registro["DataDeFinalizacao"]),
                Preco = Convert.ToDecimal(registro["Preco"])
            };
            return a;
        }

        public AluguelViewModel Consulta(int id)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id", id)
            };

            DataTable tabela = HelperDAO.ExecutaProcSelect("spConsulta", p);

            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
        }

        public List<AluguelViewModel> Listagem()
        {
            List<AluguelViewModel> lista = new List<AluguelViewModel>();

            DataTable tabela = HelperDAO.ExecutaProcSelect("spListagem", null);

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));
            return lista;
        }

        public int ProximoId()
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("tabela", "Aluguel")
            };

            DataTable tabela = HelperDAO.ExecutaProcSelect("spProximoId", p);
            return Convert.ToInt32(tabela.Rows[0]["MAIOR"]);
        }

        protected override void SetTabela()
        {
            Tabela = "Aluguel";
        }

    }
}