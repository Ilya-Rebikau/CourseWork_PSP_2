start "Build CourseWork.Models" /D"%~dp0src\CourseWork.Models" dotnet build
start "Build CourseWork.Web" /D"%~dp0src\CourseWork.Web" dotnet build
start "Build CourseWork.DistributionAPI" /D"%~dp0src\CourseWork.DistributionAPI" dotnet build
start "Build CourseWork.ComputingAPI" /D"%~dp0src\CourseWork.ComputingAPI" dotnet build
TIMEOUT 4
start "Run CourseWork.Web" /D"%~dp0src\CourseWork.Web" dotnet run --launch-profile "CourseWork.Web" --urls "http://localhost:5000"
start "Run CourseWork.DistributionAPI" /D"%~dp0src\CourseWork.DistributionAPI" dotnet run --launch-profile "CourseWork.DistributionAPI" --urls "http://localhost:5001"
start "Run CourseWork.ComputingAPI_1" /D"%~dp0src\CourseWork.ComputingAPI" dotnet run --launch-profile "CourseWork.ComputingAPI" --urls "http://localhost:5002"
start "Run CourseWork.ComputingAPI_2" /D"%~dp0src\CourseWork.ComputingAPI" dotnet run --launch-profile "CourseWork.ComputingAPI" --urls "http://localhost:5003"
start "Run CourseWork.ComputingAPI_3" /D"%~dp0src\CourseWork.ComputingAPI" dotnet run --launch-profile "CourseWork.ComputingAPI" --urls "http://localhost:5004"
start "Run CourseWork.ComputingAPI_4" /D"%~dp0src\CourseWork.ComputingAPI" dotnet run --launch-profile "CourseWork.ComputingAPI" --urls "http://localhost:5005"
start "Run CourseWork.ComputingAPI_5" /D"%~dp0src\CourseWork.ComputingAPI" dotnet run --launch-profile "CourseWork.ComputingAPI" --urls "http://localhost:5006"
start "Run CourseWork.ComputingAPI_6" /D"%~dp0src\CourseWork.ComputingAPI" dotnet run --launch-profile "CourseWork.ComputingAPI" --urls "http://localhost:5007"
start "Run CourseWork.ComputingAPI_7" /D"%~dp0src\CourseWork.ComputingAPI" dotnet run --launch-profile "CourseWork.ComputingAPI" --urls "http://localhost:5008"
start "Run CourseWork.ComputingAPI_8" /D"%~dp0src\CourseWork.ComputingAPI" dotnet run --launch-profile "CourseWork.ComputingAPI" --urls "http://localhost:5009"
start "Run CourseWork.ComputingAPI_9" /D"%~dp0src\CourseWork.ComputingAPI" dotnet run --launch-profile "CourseWork.ComputingAPI" --urls "http://localhost:5010"
start "Run CourseWork.ComputingAPI_10" /D"%~dp0src\CourseWork.ComputingAPI" dotnet run --launch-profile "CourseWork.ComputingAPI" --urls "http://localhost:5011"
