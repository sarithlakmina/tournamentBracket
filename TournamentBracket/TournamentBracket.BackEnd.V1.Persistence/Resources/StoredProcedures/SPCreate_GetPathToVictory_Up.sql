IF EXISTS ( SELECT * FROM sysobjects WHERE  id = object_id(N'[dbo].[GetPathToVictory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
	DROP PROCEDURE [dbo].GetPathToVictory
END
GO

CREATE PROCEDURE [dbo].[GetPathToVictory]
(
	@TournamentID uniqueidentifier,
	@TeamID uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #OpponentTeams (TeamName varchar(50), MatchCategoryName nvarchar(10));


	INSERT INTO #OpponentTeams (TeamName, MatchCategoryName)
    SELECT
        CASE
            WHEN m.HomeTeamID = @TeamID THEN team1.Name
            ELSE team2.Name
        END AS TeamName,
        mc.Name AS MatchCategoryName
    FROM Matches m
    LEFT JOIN Teams team1 ON m.AwayTeamID = team1.TeamID
    LEFT JOIN Teams team2 ON m.HomeTeamID = team2.TeamID
    LEFT JOIN MatchMatchCategoryMaps mmc ON m.MatchID = mmc.MatchID
    LEFT JOIN MatchCategories mc ON mmc.MatchCategoryID = mc.MatchCategoryID
    WHERE m.TournamentID = @TournamentID AND (m.AwayTeamID = @TeamID OR m.HomeTeamID = @TeamID);

SELECT * FROM #OpponentTeams;

DROP TABLE #OpponentTeams;	

END