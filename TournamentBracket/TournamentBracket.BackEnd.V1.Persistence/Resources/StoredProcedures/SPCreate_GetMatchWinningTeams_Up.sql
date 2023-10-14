IF EXISTS ( SELECT * FROM sysobjects WHERE  id = object_id(N'[dbo].[GetMatchWinnersData]') and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
	DROP PROCEDURE [dbo].GetMatchWinnersData
END
GO

CREATE PROCEDURE [dbo].[GetMatchWinnersData]
(
	@TournamentID uniqueidentifier,
	@MatchCategoryName nvarchar(50),
	@MatchCategoryID uniqueIdentifier
)
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #MatchIDs (MatchID uniqueIdentifier);
   
	SELECT @MatchCategoryID = mc.MatchCategoryID from MatchCategories as mc where  mc.MatchTypeName = @MatchCategoryName and mc.TournamentID =@TournamentID

	INSERT INTO #MatchIDs (MatchID) SELECT mcm.MatchID FROM MatchMatchCategoryMaps mcm
	WHERE mcm.MatchCategoryID = @MatchCategoryID AND mcm.TournamentID = @TournamentID;

	select m.WinningTeamID from Matches m where m.MatchID in (Select * from #MatchIDs) and m.IsMatchCompleted = 1 and m.TournamentID =@TournamentID

	select t.Seed, t.Name, t.TeamID from Teams t where t.TeamID in  (select m.WinningTeamID from Matches m where m.MatchID in (Select * from #MatchIDs) and m.IsMatchCompleted = 1 and m.TournamentID =@TournamentID)

	DROP TABLE #MatchIDs; 

END