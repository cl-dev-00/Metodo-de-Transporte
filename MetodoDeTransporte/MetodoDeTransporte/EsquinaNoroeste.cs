﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace MetodoDeTransporte
{
    class EsquinaNoroeste
    {
        private double resultado;
        private bool seguir;
        private DataGridView dgvTabla;

        private double[] ofertaArray;
        private double[] demandaArray;
        
        private double ofertaTotal = 0;
        private double demandaTotal = 0;
       
        private int esquinaColumna = 0;
        private int esquinaFila = 0;
        private int filas = 0;
        private int columnas =  0;

        private Datos[ , ] datos;

        public EsquinaNoroeste()
        {

        }

        public double calcularResultado(DataGridView tabla)
        {
            dgvTabla = tabla;
            ofertaTotal = totalOferta().Sum();
            demandaTotal = totalDemanda().Sum();

            if (ofertaTotal == demandaTotal)
            {
                filas = tabla.Rows.Count;
                columnas = tabla.Columns.Count;

                esquinaColumna = 0;
                esquinaFila = 0;

                //obtengo las ofertas y demandas de la tabla de forma indiviual
                ofertaArray = totalOferta();
                demandaArray = totalDemanda();

                MessageBox.Show(ofertaArray.Length + "x" + demandaArray.Length);

                //obtengo los datos de la tabla y los ingresos al campo cantidad de los objetos datos
                obtenerDatosTabla();

                do
                {
                    seguir = calcular();

                } while (!seguir);

                resultado = resultadoFinal();

                return resultado;

            } else {
                MessageBox.Show("La oferta y la demanda no esta balanceadas\nPor lo tanto no se puede realizar la operacion", "Ërror", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
                return 0;
            }
            
        }

        private bool calcular()
        {
            if(esquinaFila < datos.GetLength(0) || esquinaColumna < datos.GetLength(1))
            {
                double ofertaActual = ofertaArray[esquinaFila];
                double demandaActual = demandaArray[esquinaColumna];

                if (ofertaActual == demandaActual)
                {
                        datos[esquinaFila, esquinaColumna].Precio = demandaActual;
                        datos[esquinaFila, esquinaColumna].Llena = true;
                        demandaArray[esquinaColumna] = 0;
                        ofertaArray[esquinaFila] = 0;
                        esquinaColumna++;
                        esquinaFila++;
                }
                
                else if (demandaActual > ofertaActual)
                {
                    demandaActual -= ofertaActual;
                    datos[esquinaFila, esquinaColumna].Precio = demandaActual;
                    
                    demandaArray[esquinaColumna] = demandaActual;
                    ofertaArray[esquinaFila] = 0;
                    esquinaFila++;
                }

                else if(ofertaActual > demandaActual)
                {
                    ofertaActual -= demandaActual;
                    datos[esquinaFila, esquinaColumna].Precio = ofertaActual;

                    ofertaArray[esquinaFila] = ofertaActual;
                    demandaArray[esquinaColumna] = 0;
                    esquinaColumna++;
                }

                return false;
            }
            else
            {
                return true;
            }
            
        }

        private double resultadoFinal()
        {
            double totalCosto = 0;

            for(int i = 0; i < datos.GetLength(0); i++)
            {
                for (int j = 0; j < datos.GetLength(1); j++)
                {
                    if(datos[i, j].Precio>0)
                    {
                        totalCosto += (datos[i, j].Cantidad * datos[i, j].Precio); 
                    }
                }
            }

            return totalCosto;
        }

        private double[] totalOferta()
        {
            double[] oferta = new double[dgvTabla.Rows.Count-2];

            for (int i = 0; i < oferta.Length; i++)
            {
                if (dgvTabla.Rows[i].Cells[dgvTabla.Columns.Count - 1].Value != null)
                {
                   oferta[i] = Convert.ToDouble(dgvTabla.Rows[i].Cells[dgvTabla.Columns.Count - 1].Value.ToString());
                }
            }

            return oferta;
        }

        private double[] totalDemanda()
        {
            double[] demanda = new double[dgvTabla.Columns.Count-2];

            for (int i = 1; i < dgvTabla.Columns.Count-1; i++)
            {
                if (dgvTabla.Rows[dgvTabla.Rows.Count - 2].Cells[i].Value != null)
                {
                    demanda[i - 1] = Convert.ToDouble(dgvTabla.Rows[dgvTabla.Rows.Count - 2].Cells[i].Value.ToString());
                }
            }


            return demanda;
        }

        //el metodo obtiene los datos de la tabla y los ingrese al campo cantidad de los objetos datos
        private void obtenerDatosTabla()
        {
            datos = new Datos[dgvTabla.Rows.Count-2, dgvTabla.Columns.Count - 2];
            MessageBox.Show(datos.GetLength(0) + "-"+ datos.GetLength(1));

            for(int i = 0; i < datos.GetLength(0); i++)
            {
                for(int j = 0; j < datos.GetLength(1); j++)
                {

                    datos[i, j] = new Datos();
                    datos[i, j].Cantidad = Convert.ToInt32(dgvTabla.Rows[i].Cells[j+1].Value.ToString());
                }
            }
            MessageBox.Show("Fin");

        }
    }

}