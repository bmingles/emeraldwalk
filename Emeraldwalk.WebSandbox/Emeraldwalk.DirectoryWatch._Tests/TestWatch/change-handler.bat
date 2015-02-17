SETLOCAL
pushd %~dp0

echo ******************* Something Changed ********************** >> log.txt

SET change_type=%1
SET object_type=%2
SET changed=%3
SET changed_nox=%4
SET changed_rel=%5
SET original=%6
SET each=%7
SET each_nox=%8
SET each_rel=%9

ECHO %change_type% >> log.txt
ECHO %object_type% >> log.txt
ECHO %changed% >> log.txt
ECHO %changed_nox% >> log.txt
ECHO %changed_rel% >> log.txt
ECHO %original% >> log.txt
ECHO %each% >> log.txt
ECHO %each_nox% >> log.txt
ECHO %each_rel% >> log.txt

IF %object_type%=="File" (
	GOTO :FILE 
) ELSE (
	GOTO :DIR
)

::File Processing
:FILE

ECHO File >> log.txt

IF %change_type%=="Create" (
	ECHO Create >> log.txt
) ELSE IF %change_type%=="Change" (
	ECHO Change >> log.txt
) ELSE IF %change_type%=="Rename" (
	ECHO Rename %original% to %changed% >> log.txt
) ELSE IF %change_type%=="Delete" (
	ECHO Delete >> log.txt
)

GOTO :END


::Dir Processing
:DIR

ECHO Dir >> log.txt

IF %change_type%=="Create" (
	ECHO Create >> log.txt
) ELSE IF %change_type%=="Change" (
	ECHO Change >> log.txt
) ELSE IF %change_type%=="Rename" (
	ECHO Rename >> log.txt
) ELSE IF %change_type%=="Delete" (
	ECHO Delete >> log.txt
)

GOTO :END

:END

popd
ENDLOCAL