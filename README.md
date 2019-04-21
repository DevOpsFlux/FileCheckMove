# UploadBatch.exe Excute Program 
- File Check Local File -> IMG Server Move

## 1. 프로젝트 정보 및 버젼

### *[ UploadBatch Solution ]	
### *[ UploadBatch.csproj ]	

| 프로젝트 | 설명 | .NET버젼 | UploadBatch버젼 |
| -------- | -------- | -------- | -------- |
| UploadBatch | 이미지 체크 이동	| .NET 4.0	| UploadBatch.exe 1.0.0.0 |

## 2. UploadBatch 정보 및 참조
- SEED 암/복호화 모듈 ECB/CBC MODE
- CryptoNetCom.dll 1.0.0.0
- System.EnterpriseServices

## 3. UploadBatch 사용 정보
* IIS Log : C:\inetpub\logs\LogFiles\W3SVC2
* UploadBatch.exe 실행 > IIS Log 파일 지정 > 검색 데이터 지정 후 검색 실행 > LogView 확인

* 4. upf.config 설정 :
```
	<dir>D:\ImgDir\TargetDir\</dir> 
	<nas_dir>\\mynas.flux.com\image\TargetDir\</nas_dir>
 	<log>D:\comlog\UPF\</log>
	<ExceptDir>artisttestx;</ExceptDir>
	<WhiteList>gif;jpg;jpeg;pjpeg;png;JPG;</WhiteList>
	<BlackList>asp;vbs;php;jsp;cer;cdx;asa;htr;</BlackList>
```
