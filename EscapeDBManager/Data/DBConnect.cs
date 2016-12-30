using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace EscapeDBManager.Data
{
    public class DBConnect
    {

        private MySqlConnection _conn; 

        public DBConnect() {
            _conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }

        public DataSet GetRoomList()
        {
            DataSet retVal = null;
            try
            {
                _conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from Room", _conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");
                retVal = ds;
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.ToString());
                //LOG ERROR
                
            }
            finally
            {
                _conn.Close();
                
            }
            return retVal;
        }

        public int CreateNewGame(string Name, int CustomerID, string Description)
        {
            int newGameID = -1;
            try
            {
                _conn.Open();
                MySqlCommand cmd = new MySqlCommand("NewGame", _conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("GameName",Name);
                cmd.Parameters.AddWithValue("CustomerID", CustomerID);
                cmd.Parameters.AddWithValue("Description", Description);
                MySqlParameter param = cmd.CreateParameter();
                param.ParameterName = "AddedGameID";
                param.Direction = ParameterDirection.Output;
                param.MySqlDbType = MySqlDbType.Int32;
                //cmd.Parameters.Add("AddedGameID", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);

                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");

                //Must be a better way to do this
                newGameID = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0]);
                
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.ToString());
                //LOG ERROR

            }
            finally
            {
                _conn.Close();

            }
            return newGameID;

        }

        public bool AddTeamToGame(int GameID, string TeamName)
        {
            bool bRetVal = false;
            try
            {
                _conn.Open();
                MySqlCommand cmd = new MySqlCommand("AddTeamToGame", _conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("GameID", GameID);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("TeamName", TeamName);

                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");

                bRetVal = true;

            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.ToString());
                //LOG ERROR

            }
            finally
            {
                _conn.Close();

            }
            return bRetVal;

        }


        public int GenerateGameRounds(int GameID)
        {
            int nRetVal = 0;
            try
            {
                _conn.Open();
                MySqlCommand cmd = new MySqlCommand("GenerateGameRounds", _conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("GameID", GameID);

                MySqlParameter param = cmd.CreateParameter();
                param.ParameterName = "RoundAddedCount";
                param.Direction = ParameterDirection.Output;
                param.MySqlDbType = MySqlDbType.Int32;
                //cmd.Parameters.Add("AddedGameID", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);

                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");

                //Must be a better way to do this
                int nCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0]);

                nRetVal = nCount;

            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.ToString());
                //LOG ERROR

            }
            finally
            {
                _conn.Close();

            }
            return nRetVal;

        }
        public DataSet GetRoundList(int GameID)
        {
            DataSet retVal = null;
            try
            {
                _conn.Open();
                //           MySqlCommand cmd = new MySqlCommand("Select * from Round where GameID="+GameID, _conn);
                MySqlCommand cmd = new MySqlCommand("GetRounds", _conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("inGameID", GameID);

                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");
                retVal = ds;
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.ToString());
                //LOG ERROR

            }
            finally
            {
                _conn.Close();

            }
            return retVal;
        }

        public bool StartRound(int RoundID)
        {
            bool bRetVal = false;
            try
            {
                _conn.Open();
                MySqlCommand cmd = new MySqlCommand("StartRound", _conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("inRoundID", RoundID);

                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");

                bRetVal = true;

            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.ToString());
                //LOG ERROR

            }
            finally
            {
                _conn.Close();

            }
            return bRetVal;

        }

        public bool EndRound(int GameID, int RoundID)
        {
            bool bRetVal = false;
            try
            {
                _conn.Open();
                MySqlCommand cmd = new MySqlCommand("EndRound", _conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("inGameID", GameID);
                cmd.Parameters.AddWithValue("inRoundID", RoundID);

                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");

                bRetVal = true;

            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.ToString());
                //LOG ERROR

            }
            finally
            {
                _conn.Close();

            }
            return bRetVal;

        }

        public DataSet GetActiveRoundList(int GameID)
        {
            DataSet retVal = null;
            try
            {
                _conn.Open();
                //           MySqlCommand cmd = new MySqlCommand("Select * from Round where GameID="+GameID, _conn);
                MySqlCommand cmd = new MySqlCommand("GetActiveRounds", _conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("inGameID", GameID);

                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");
                retVal = ds;
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.ToString());
                //LOG ERROR

            }
            finally
            {
                _conn.Close();

            }
            return retVal;
        }

        public bool SetHintsUsed(int GameID, int RoundID, int HintsUsed)
        {
            bool bRetVal = false;
            try
            {
                _conn.Open();
                MySqlCommand cmd = new MySqlCommand("SetHintsUsed", _conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("inGameID", GameID);
                cmd.Parameters.AddWithValue("inRoundID", RoundID);
                cmd.Parameters.AddWithValue("inHintsUsed", HintsUsed);

                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");

                bRetVal = true;

            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.ToString());
                //LOG ERROR

            }
            finally
            {
                _conn.Close();

            }
            return bRetVal;

        }

        public bool AddPlusTime(int GameID, int RoundID, int Minutes)
        {
            bool bRetVal = false;
            try
            {
                _conn.Open();
                MySqlCommand cmd = new MySqlCommand("AddPlusTime", _conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("inGameID", GameID);
                cmd.Parameters.AddWithValue("inRoundID", RoundID);
                cmd.Parameters.AddWithValue("inMinutesDelta", Minutes);

                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");

                bRetVal = true;

            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.ToString());
                //LOG ERROR

            }
            finally
            {
                _conn.Close();

            }
            return bRetVal;

        }
        public bool SendMessage(int GameID, string msg)
        {
            bool bRetVal = false;
            try
            {
                _conn.Open();
                MySqlCommand cmd = new MySqlCommand("SendMessage", _conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("inGameID", GameID);
                cmd.Parameters.AddWithValue("inMessageText", msg);

                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");

                bRetVal = true;

            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.ToString());
                //LOG ERROR

            }
            finally
            {
                _conn.Close();

            }
            return bRetVal;

        }

    }
}
