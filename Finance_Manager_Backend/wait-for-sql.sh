echo "Waiting for MSSQL to be ready..."

until /opt/mssql-tools/bin/sqlcmd -S db -U ${DB_USER} -P ${DB_PASSWORD} -Q "SELECT 1" > /dev/null 2>&1
do
  sleep 2
done

echo "MSSQL is ready."
