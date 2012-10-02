truncate table OrgUnitCTS;
insert into OrgUnitCTS (ShortName, Name, HrmsOrgUnitId)
(select ou.OrgCodeLegacy, ou.OrgUnitDescription, Id
from Organization.dbo.OrgUnit ou)
select * from OrgUnitCTS

-- OrgUnitCTS detail:
--   HRMS OrgUnitId (alpha dropdown of Descriptions)
--   SupervisorPositionId (alpha dropdown)
--   HRMS Position Count
--   Cost Center
--   LegacyUnitId

-- TODO:
--    insert / update / logical delete from Gap data
--    block delete if associated Positions exist   

insert into OrgUnitNodeCTS
select ouc.Id, pou.Id as ParentId, oun.Ancestry from
OrgUnitCTS ouc
join
OrgUnit ou
on ouc.HrmsOrgUnitId = ou.Id
join OrgUnitNode oun
on oun.Id = ou.Id
left join OrgUnitCTS pou
on pou.HrmsOrgUnitId = oun.ParentId

select
--replicate(' . ', datalength(ancestry)) + ou.ShortName, 
replicate(' . ', datalength(ancestry)) + ou.Name, 
replicate(' . ', datalength(ancestry)) + ouo.Name
from OrgUnitNodeCTS oun
join OrgUnitCTS ou
on oun.Id = ou.Id
join OrgUnit ouo
on ou.HrmsOrgUnitId = ouo.Id
order by ancestry

