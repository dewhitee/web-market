WITH duplicates AS (
	SELECT
		ID,
		ProductID,
		TypeId,
		ROW_NUMBER() OVER (
			PARTITION BY
				ProductID,
				TypeId
			ORDER BY
				ProductID,
				TypeId
		) row_num
	FROM
		dbo.Tags
)
DELETE FROM duplicates
WHERE row_num > 1;