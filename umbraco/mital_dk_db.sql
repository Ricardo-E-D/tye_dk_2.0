-- phpMyAdmin SQL Dump
-- version 3.5.0
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Jan 21, 2013 at 04:17 PM
-- Server version: 5.5.19
-- PHP Version: 5.2.6

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `mital_dk_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `CMSCONTENT`
--

CREATE TABLE IF NOT EXISTS `CMSCONTENT` (
  `PK` int(11) NOT NULL AUTO_INCREMENT,
  `NODEID` int(11) NOT NULL,
  `CONTENTTYPE` int(11) NOT NULL,
  PRIMARY KEY (`PK`),
  KEY `NODEID` (`NODEID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=11 ;

--
-- Dumping data for table `CMSCONTENT`
--

INSERT INTO `CMSCONTENT` (`PK`, `NODEID`, `CONTENTTYPE`) VALUES
(1, 1053, 1044),
(2, 1059, 1058),
(3, 1060, 1056),
(4, 1062, 1057),
(5, 1067, 1063),
(6, 1068, 1065),
(7, 1069, 1065),
(8, 1071, 1046),
(9, 1076, 1074),
(10, 1078, 1077);

-- --------------------------------------------------------

--
-- Table structure for table `CMSCONTENTTYPE`
--

CREATE TABLE IF NOT EXISTS `CMSCONTENTTYPE` (
  `PK` int(11) NOT NULL AUTO_INCREMENT,
  `NODEID` int(11) NOT NULL,
  `ALIAS` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `ICON` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `THUMBNAIL` varchar(255) CHARACTER SET utf8 NOT NULL DEFAULT 'folder.png',
  `DESCRIPTION` varchar(1500) CHARACTER SET utf8 DEFAULT NULL,
  `MASTERCONTENTTYPE` int(11) DEFAULT NULL,
  PRIMARY KEY (`PK`),
  KEY `IX_ICON` (`NODEID`,`ICON`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=548 ;

--
-- Dumping data for table `CMSCONTENTTYPE`
--

INSERT INTO `CMSCONTENTTYPE` (`PK`, `NODEID`, `ALIAS`, `ICON`, `THUMBNAIL`, `DESCRIPTION`, `MASTERCONTENTTYPE`) VALUES
(532, 1031, 'Folder', 'folder.gif', 'folder.png', NULL, NULL),
(533, 1032, 'Image', 'mediaPhoto.gif', 'folder.png', NULL, NULL),
(534, 1033, 'File', 'mediaFile.gif', 'folder.png', NULL, NULL),
(535, 1044, 'Administrator', 'member.gif', 'folder.png', NULL, NULL),
(536, 1045, 'Partner', 'member.gif', 'folder.png', NULL, NULL),
(537, 1046, 'Optician', 'member.gif', 'folder.png', '', NULL),
(538, 1047, 'Client', 'member.gif', 'folder.png', NULL, NULL),
(541, 1056, 'Exercise', 'doc2.gif', 'folder.png', '', NULL),
(542, 1057, 'ExerciseInfo', 'doc3.gif', 'folder.png', '', NULL),
(543, 1058, 'ExerciseFolder', 'folder.gif', 'folder.png', '', NULL),
(544, 1063, 'master', 'settingDomain.gif', 'folder.png', '', NULL),
(545, 1065, 'page', 'doc.gif', 'doc.png', '', 1063),
(546, 1074, 'ExerciseLog', 'folder.gif', 'folder.png', '', NULL),
(547, 1077, 'ExerciseLogFolder', 'folder.gif', 'folder.png', '', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `CMSCONTENTTYPEALLOWEDCONTENTTYPE`
--

CREATE TABLE IF NOT EXISTS `CMSCONTENTTYPEALLOWEDCONTENTTYPE` (
  `ID` int(11) NOT NULL,
  `ALLOWEDID` int(11) NOT NULL,
  PRIMARY KEY (`ID`,`ALLOWEDID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci;

--
-- Dumping data for table `CMSCONTENTTYPEALLOWEDCONTENTTYPE`
--

INSERT INTO `CMSCONTENTTYPEALLOWEDCONTENTTYPE` (`ID`, `ALLOWEDID`) VALUES
(1031, 1031),
(1031, 1032),
(1031, 1033),
(1056, 1057),
(1058, 1056),
(1063, 1065),
(1065, 1065),
(1077, 1074);

-- --------------------------------------------------------

--
-- Table structure for table `CMSCONTENTVERSION`
--

CREATE TABLE IF NOT EXISTS `CMSCONTENTVERSION` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `CONTENTID` int(11) NOT NULL,
  `VERSIONID` char(36) COLLATE latin1_danish_ci NOT NULL,
  `VERSIONDATE` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ID`),
  KEY `IX_CONTENTID_VERSIONDATE` (`CONTENTID`,`VERSIONDATE`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=31 ;

--
-- Dumping data for table `CMSCONTENTVERSION`
--

INSERT INTO `CMSCONTENTVERSION` (`ID`, `CONTENTID`, `VERSIONID`, `VERSIONDATE`) VALUES
(1, 1053, '181631a3-65e0-4aa1-8c27-5787748b94a7', '2012-11-09 15:48:06'),
(2, 1059, '2f16dfa5-751a-4c40-83aa-1f1f640b5bf9', '2012-11-09 16:36:58'),
(3, 1060, '1f071181-5511-40ea-9be7-e44a86657f5a', '2012-11-09 19:28:02'),
(4, 1062, 'c3242cd5-7053-4a7a-89a0-a5e23f53e459', '2012-11-09 19:56:34'),
(5, 1059, 'e0b45beb-3a6d-4574-81ea-bd67c7498887', '2012-11-09 20:00:36'),
(6, 1060, 'a42426d9-c409-484a-b7f9-ed7e85661306', '2012-11-09 20:00:36'),
(7, 1062, '052bd737-216b-48e1-b09f-69189d6641f1', '2012-11-09 20:00:36'),
(8, 1060, '2ea61431-3272-40df-ba03-443f02a5da2a', '2012-11-09 20:01:55'),
(9, 1060, '6606919f-5cb4-497f-8161-c9be3d4707dd', '2012-11-09 20:02:35'),
(10, 1067, 'e1946700-041b-4f62-81a3-e9d106d602d8', '2012-11-09 20:13:41'),
(11, 1067, 'a22e5d76-7e6b-4dc4-8412-0761629f5ef6', '2012-11-09 21:56:54'),
(12, 1068, 'e77253ce-4b4c-493f-94c7-327b0c2bb9a3', '2012-11-09 21:59:35'),
(13, 1068, '38db764b-695d-4d5c-be69-49fe6e112328', '2012-11-09 22:02:05'),
(14, 1067, '6b734643-6548-4860-8504-abd705d055d9', '2012-11-09 22:02:51'),
(15, 1069, '8bab687b-39c8-44ae-8aa5-e4e4255a953b', '2012-11-09 22:08:03'),
(16, 1069, '36ef08f8-adea-4165-b19c-20c9523bd6a0', '2012-11-09 22:08:12'),
(17, 1069, 'ee5733f9-e2a0-49e4-ad1c-517f7790027e', '2012-11-09 22:09:13'),
(18, 1069, 'bd1ef6c4-66b3-4c22-924f-b4bc5c1c9574', '2012-11-09 22:09:33'),
(19, 1067, '1d48a244-1f86-4acb-b83e-c70d8c79a5ad', '2012-11-09 22:09:39'),
(20, 1069, 'bdbb98d8-041e-470e-b1ec-f724c6a82400', '2012-11-09 22:10:26'),
(21, 1068, '2aa77de6-dc62-4dea-9752-e2b0795635b2', '2012-11-09 22:10:35'),
(22, 1060, 'a84ebf5e-7a50-4fed-8eb7-c75622b4cd87', '2012-11-09 22:15:46'),
(23, 1060, '026adef7-e973-495c-990c-7f9a5b00f25a', '2012-11-10 07:11:18'),
(24, 1071, 'd476e5c8-0d24-477a-bb75-5271c18da8f3', '2012-11-10 07:28:21'),
(25, 1076, '91a03346-79e3-48a3-9a87-ef098b6bca96', '2012-11-10 15:52:08'),
(26, 1078, 'e2bfc460-a3b8-4112-97e8-340cc9c9ea2f', '2012-11-10 16:00:54'),
(27, 1078, '9be14cc0-7b81-4045-971b-f7465d770783', '2012-11-10 16:01:13'),
(28, 1076, 'ab694aa2-bf3e-4c8b-b0b4-177dbfa04fd1', '2012-11-10 16:01:17'),
(29, 1062, 'ad9fbc13-e861-4441-b27f-e6e617fcfb16', '2012-11-10 16:02:57'),
(30, 1062, '7dbbb112-640f-48f0-8c8c-b31f8811983a', '2012-11-10 16:06:47');

-- --------------------------------------------------------

--
-- Table structure for table `CMSCONTENTXML`
--

CREATE TABLE IF NOT EXISTS `CMSCONTENTXML` (
  `NODEID` int(11) NOT NULL,
  `XML` longtext COLLATE latin1_danish_ci NOT NULL,
  PRIMARY KEY (`NODEID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci;

--
-- Dumping data for table `CMSCONTENTXML`
--

INSERT INTO `CMSCONTENTXML` (`NODEID`, `XML`) VALUES
(1053, '<node id="1053" version="181631a3-65e0-4aa1-8c27-5787748b94a7" parentID="-1" level="1" writerID="0" nodeType="1044" template="0" sortOrder="0" createDate="2012-11-09T16:48:06" updateDate="2012-11-09T16:48:06" nodeName="a" urlName="a" writerName="monosolutions" nodeTypeAlias="Administrator" path="-1,1053" loginName="monosolutions" email="contact@monosolutions.dk" />'),
(1059, '<ExerciseFolder id="1059" parentID="-1" level="1" writerID="0" creatorID="0" nodeType="1058" template="0" sortOrder="1" createDate="2012-11-09T17:36:58" updateDate="2012-11-09T21:00:36" nodeName="Exercises" urlName="exercises" writerName="monosolutions" creatorName="monosolutions" path="-1,1059" isDoc="" />'),
(1060, '<Exercise id="1060" parentID="1059" level="2" writerID="0" creatorID="0" nodeType="1056" template="0" sortOrder="0" createDate="2012-11-09T20:28:02" updateDate="2012-11-10T08:11:18" nodeName="Vestibulærtræning" urlName="vestibulaertraening" writerName="monosolutions" creatorName="monosolutions" path="-1,1059,1060" isDoc=""><type>6</type><nameDk>Vestibulærtræning</nameDk><nameDe></nameDe><nameGb></nameGb><nameNo></nameNo><textDk><![CDATA[<p><span>Formål:</span></p>\r\n<ol>\r\n<li>At få en bedre balance.</li>\r\n<li>At blive <strong>bedre</strong> til at klare svimmelhed.</li>\r\n</ol>]]></textDk><textDe><![CDATA[<p><strong>asdfasdf</strong></p>]]></textDe><textGb><![CDATA[]]></textGb><textNo><![CDATA[]]></textNo></Exercise>'),
(1062, '<ExerciseInfo id="1062" parentID="1060" level="3" writerID="0" creatorID="0" nodeType="1057" template="0" sortOrder="0" createDate="2012-11-09T20:56:34" updateDate="2012-11-10T17:06:47" nodeName="Start" urlName="start" writerName="monosolutions" creatorName="monosolutions" path="-1,1059,1060,1062" isDoc=""><headingDk>Intro</headingDk><headingDe>Intro</headingDe><headingGb>Intro</headingGb><headingNo>Intro</headingNo><textDk><![CDATA[<ol>\r\n<li>Denne øvelse er opdelt i 7 forskellige trin, der træner hver deres del af labyrintsansen. Læs evt. mere om labyrintsansen under balancen.</li>\r\n<li>Har man problemer med balancen skal man lave denne øvelse i små portioner, da de kan fremkalde svimmelhed og ubehag.</li>\r\n<li>Det er ikke sikkert, at man har problemer med alle delene. Hvis man kan lave to af trinene uden problemer, skal man bare gå videre til næste.</li>\r\n</ol>]]></textDk><textDe><![CDATA[]]></textDe><textGb><![CDATA[]]></textGb><textNo><![CDATA[]]></textNo></ExerciseInfo>'),
(1067, '<master id="1067" parentID="-1" level="1" writerID="0" creatorID="0" nodeType="1063" template="1066" sortOrder="0" createDate="2012-11-09T21:13:41" updateDate="2012-11-09T23:09:39" nodeName="Forside" urlName="forside" writerName="monosolutions" creatorName="monosolutions" path="-1,1067" isDoc=""><seoPageTitle></seoPageTitle><seoKeywords></seoKeywords><seoDescription></seoDescription><bodyText><![CDATA[<p>text</p>]]></bodyText></master>'),
(1068, '<page id="1068" parentID="1067" level="2" writerID="0" creatorID="0" nodeType="1065" template="1066" sortOrder="0" createDate="2012-11-09T22:59:35" updateDate="2012-11-09T23:10:35" nodeName="Test dine øjne" urlName="test-dine-oejne" writerName="monosolutions" creatorName="monosolutions" path="-1,1067,1068" isDoc=""><seoPageTitle></seoPageTitle><seoKeywords></seoKeywords><seoDescription></seoDescription><bodyText><![CDATA[<p>haloooo!</p>]]></bodyText></page>'),
(1069, '<page id="1069" parentID="1067" level="2" writerID="0" creatorID="0" nodeType="1065" template="1066" sortOrder="1" createDate="2012-11-09T23:08:03" updateDate="2012-11-09T23:10:26" nodeName="TYE" urlName="tye" writerName="monosolutions" creatorName="monosolutions" path="-1,1067,1069" isDoc=""><seoPageTitle></seoPageTitle><seoKeywords></seoKeywords><seoDescription></seoDescription><bodyText><![CDATA[<p>snaps!</p>]]></bodyText></page>'),
(1071, '<node id="1071" version="d476e5c8-0d24-477a-bb75-5271c18da8f3" parentID="-1" level="1" writerID="0" nodeType="1046" template="0" sortOrder="2" createDate="2012-11-10T08:28:03" updateDate="2012-11-10T08:28:21" nodeName="aa - optician" urlName="aa-optician" writerName="monosolutions" nodeTypeAlias="Optician" path="-1,1071" loginName="aa" email="aa@aa.com"><country><![CDATA[Denmark]]></country></node>'),
(1076, '<ExerciseLog id="1076" parentID="1078" level="2" writerID="0" creatorID="0" nodeType="1074" template="0" sortOrder="1" createDate="2012-11-10T16:52:08" updateDate="2012-11-10T17:01:17" nodeName="log 1" urlName="log-1" writerName="monosolutions" creatorName="monosolutions" path="-1,1078,1076" isDoc=""><exercise>1060</exercise><member>1053</member><timeStart>2012-11-10T04:20:00</timeStart><timeEnd>2012-11-11T11:41:00</timeEnd><note><![CDATA[]]></note><score>1</score></ExerciseLog>'),
(1078, '<ExerciseLogFolder id="1078" parentID="-1" level="1" writerID="0" creatorID="0" nodeType="1077" template="0" sortOrder="3" createDate="2012-11-10T17:00:54" updateDate="2012-11-10T17:01:13" nodeName="Logs" urlName="logs" writerName="monosolutions" creatorName="monosolutions" path="-1,1078" isDoc="" />');

-- --------------------------------------------------------

--
-- Table structure for table `CMSDATATYPE`
--

CREATE TABLE IF NOT EXISTS `CMSDATATYPE` (
  `PK` int(11) NOT NULL AUTO_INCREMENT,
  `NODEID` int(11) NOT NULL,
  `CONTROLID` char(36) COLLATE latin1_danish_ci NOT NULL,
  `DBTYPE` varchar(50) COLLATE latin1_danish_ci NOT NULL,
  PRIMARY KEY (`PK`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=47 ;

--
-- Dumping data for table `CMSDATATYPE`
--

INSERT INTO `CMSDATATYPE` (`PK`, `NODEID`, `CONTROLID`, `DBTYPE`) VALUES
(4, -49, '38b352c1-e9f8-4fd8-9324-9a2eab06d97a', 'Integer'),
(6, -51, '1413afcb-d19a-4173-8e9a-68288d2a73b8', 'Integer'),
(8, -87, '5E9B75AE-FACE-41c8-B47E-5F4B0FD82F83', 'Ntext'),
(9, -88, 'ec15c1e5-9d90-422a-aa52-4f7622c63bea', 'Nvarchar'),
(10, -89, '67db8357-ef57-493e-91ac-936d305e0f2a', 'Ntext'),
(11, -90, '5032a6e6-69e3-491d-bb28-cd31cd11086c', 'Nvarchar'),
(12, -91, 'a74ea9c9-8e18-4d2a-8cf6-73c6206c5da6', 'Nvarchar'),
(13, -92, '6c738306-4c17-4d88-b9bd-6546f3771597', 'Nvarchar'),
(14, -36, 'b6fb1622-afa5-4bbf-a3cc-d9672a442222', 'Date'),
(15, -37, 'f8d60f68-ec59-4974-b43b-c46eb5677985', 'Nvarchar'),
(16, -38, 'cccd4ae9-f399-4ed2-8038-2e88d19e810c', 'Nvarchar'),
(17, -39, '928639ed-9c73-4028-920c-1e55dbb68783', 'Nvarchar'),
(18, -40, 'a52c7c1c-c330-476e-8605-d63d3b84b6a6', 'Nvarchar'),
(19, -41, '23e93522-3200-44e2-9f29-e61a6fcbb79a', 'Date'),
(20, -42, 'a74ea9c9-8e18-4d2a-8cf6-73c6206c5da6', 'Integer'),
(21, -43, 'b4471851-82b6-4c75-afa4-39fa9c6a75e9', 'Nvarchar'),
(22, -44, 'a3776494-0574-4d93-b7de-efdfdec6f2d1', 'Ntext'),
(23, -128, 'a52c7c1c-c330-476e-8605-d63d3b84b6a6', 'Nvarchar'),
(24, -129, '928639ed-9c73-4028-920c-1e55dbb68783', 'Nvarchar'),
(25, -130, 'a74ea9c9-8e18-4d2a-8cf6-73c6206c5da6', 'Nvarchar'),
(26, -131, 'a74ea9c9-8e18-4d2a-8cf6-73c6206c5da6', 'Nvarchar'),
(27, -132, 'a74ea9c9-8e18-4d2a-8cf6-73c6206c5da6', 'Nvarchar'),
(28, -133, '6c738306-4c17-4d88-b9bd-6546f3771597', 'Ntext'),
(29, -134, '928639ed-9c73-4028-920c-1e55dbb68783', 'Nvarchar'),
(30, -50, 'aaf99bb2-dbbe-444d-a296-185076bf0484', 'Date'),
(31, 1034, '158aa029-24ed-4948-939e-c3da209e5fba', 'Integer'),
(32, 1035, 'ead69342-f06d-4253-83ac-28000225583b', 'Integer'),
(33, 1036, '39f533e4-0551-4505-a64b-e0425c5ce775', 'Integer'),
(35, 1038, '60b7dabf-99cd-41eb-b8e9-4d2e669bbde9', 'Ntext'),
(36, 1039, 'cdbf0b5d-5cb2-445f-bc12-fcaaec07cf2c', 'Ntext'),
(37, 1040, '71b8ad1a-8dc2-425c-b6b8-faa158075e63', 'Ntext'),
(38, 1041, '4023e540-92f5-11dd-ad8b-0800200c9a66', 'Ntext'),
(39, 1042, '474FCFF8-9D2D-11DE-ABC6-AD7A56D89593', 'Ntext'),
(40, 1043, '7A2D436C-34C2-410F-898F-4A23B3D79F54', 'Ntext'),
(42, 1061, 'a52c7c1c-c330-476e-8605-d63d3b84b6a6', 'Nvarchar'),
(43, 1070, 'a74ea9c9-8e18-4d2a-8cf6-73c6206c5da6', 'Nvarchar'),
(45, 1073, '00000000-0000-0000-0000-000000000000', 'Ntext'),
(46, 1075, '173a96ae-00ed-4a7c-9f76-4b53d4a0a1b9', 'Nvarchar');

-- --------------------------------------------------------

--
-- Table structure for table `CMSDATATYPEPREVALUES`
--

CREATE TABLE IF NOT EXISTS `CMSDATATYPEPREVALUES` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `DATATYPENODEID` int(11) NOT NULL,
  `VALUE` varchar(2500) CHARACTER SET utf8 DEFAULT NULL,
  `SORTORDER` int(11) NOT NULL,
  `ALIAS` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=14 ;

--
-- Dumping data for table `CMSDATATYPEPREVALUES`
--

INSERT INTO `CMSDATATYPEPREVALUES` (`ID`, `DATATYPENODEID`, `VALUE`, `SORTORDER`, `ALIAS`) VALUES
(3, -87, ',code,undo,redo,cut,copy,mcepasteword,stylepicker,bold,italic,bullist,numlist,outdent,indent,mcelink,unlink,mceinsertanchor,mceimage,umbracomacro,mceinserttable,umbracoembed,mcecharmap,|1|1,2,3,|0|500,400|1049,|true|', 0, ''),
(4, 1041, 'default', 0, 'group'),
(5, 1061, 'Screen', 1, ''),
(6, 1061, 'Text', 2, ''),
(7, 1070, 'Denmark', 1, ''),
(8, 1070, 'Germany', 2, ''),
(9, 1070, 'Austria', 0, ''),
(10, 1070, 'Schweiz', 3, ''),
(11, 1075, '{"XPath":"//ExerciseFolder/Exercise","UseId":true}', 0, ''),
(12, 1036, '', 0, ''),
(13, -51, '', 0, '');

-- --------------------------------------------------------

--
-- Table structure for table `CMSDICTIONARY`
--

CREATE TABLE IF NOT EXISTS `CMSDICTIONARY` (
  `PK` int(11) NOT NULL AUTO_INCREMENT,
  `ID` char(36) COLLATE latin1_danish_ci NOT NULL,
  `PARENT` char(36) COLLATE latin1_danish_ci NOT NULL,
  `KEY` varchar(1000) CHARACTER SET utf8 NOT NULL,
  PRIMARY KEY (`PK`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=2 ;

--
-- Dumping data for table `CMSDICTIONARY`
--

INSERT INTO `CMSDICTIONARY` (`PK`, `ID`, `PARENT`, `KEY`) VALUES
(1, 'fbcbf77d-2ca5-491c-866f-0f13bb749182', '41c7638d-f529-4bff-853e-59a0c2fb1bde', 'Important');

-- --------------------------------------------------------

--
-- Table structure for table `CMSDOCUMENT`
--

CREATE TABLE IF NOT EXISTS `CMSDOCUMENT` (
  `NODEID` int(11) NOT NULL,
  `PUBLISHED` bit(1) NOT NULL,
  `DOCUMENTUSER` int(11) NOT NULL,
  `VERSIONID` char(36) COLLATE latin1_danish_ci NOT NULL,
  `TEXT` varchar(255) CHARACTER SET utf8 NOT NULL,
  `RELEASEDATE` datetime DEFAULT NULL,
  `EXPIREDATE` datetime DEFAULT NULL,
  `UPDATEDATE` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `TEMPLATEID` int(11) DEFAULT NULL,
  `ALIAS` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `NEWEST` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`VERSIONID`),
  KEY `NODEID` (`NODEID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci;

--
-- Dumping data for table `CMSDOCUMENT`
--

INSERT INTO `CMSDOCUMENT` (`NODEID`, `PUBLISHED`, `DOCUMENTUSER`, `VERSIONID`, `TEXT`, `RELEASEDATE`, `EXPIREDATE`, `UPDATEDATE`, `TEMPLATEID`, `ALIAS`, `NEWEST`) VALUES
(1060, '\0', 0, '026adef7-e973-495c-990c-7f9a5b00f25a', 'Vestibulærtræning', NULL, NULL, '2012-11-10 07:11:18', NULL, NULL, ''),
(1062, '\0', 0, '052bd737-216b-48e1-b09f-69189d6641f1', 'Start', NULL, NULL, '2012-11-10 16:06:48', NULL, NULL, '\0'),
(1067, '\0', 0, '1d48a244-1f86-4acb-b83e-c70d8c79a5ad', 'Forside', NULL, NULL, '2012-11-09 22:09:39', 1066, NULL, ''),
(1060, '\0', 0, '1f071181-5511-40ea-9be7-e44a86657f5a', 'Vestibulærtræning', NULL, NULL, '2012-11-09 20:01:56', NULL, NULL, '\0'),
(1068, '\0', 0, '2aa77de6-dc62-4dea-9752-e2b0795635b2', 'Test dine øjne', NULL, NULL, '2012-11-10 14:39:50', 1066, NULL, ''),
(1060, '\0', 0, '2ea61431-3272-40df-ba03-443f02a5da2a', 'Vestibulærtræning', NULL, NULL, '2012-11-09 22:15:47', NULL, NULL, '\0'),
(1059, '', 0, '2f16dfa5-751a-4c40-83aa-1f1f640b5bf9', 'Exercises', NULL, NULL, '2012-11-09 20:00:37', NULL, NULL, '\0'),
(1069, '\0', 0, '36ef08f8-adea-4165-b19c-20c9523bd6a0', 'TYE', NULL, NULL, '2012-11-09 22:09:34', 1066, NULL, '\0'),
(1068, '', 0, '38db764b-695d-4d5c-be69-49fe6e112328', 'Test dine øjne', NULL, NULL, '2012-11-09 22:10:36', 1066, NULL, '\0'),
(1060, '\0', 0, '6606919f-5cb4-497f-8161-c9be3d4707dd', 'Vestibulærtræning', NULL, NULL, '2012-11-10 07:11:18', NULL, NULL, '\0'),
(1067, '', 0, '6b734643-6548-4860-8504-abd705d055d9', 'Forside', NULL, NULL, '2012-11-09 22:09:39', 1066, NULL, '\0'),
(1062, '\0', 0, '7dbbb112-640f-48f0-8c8c-b31f8811983a', 'Start', NULL, NULL, '2012-11-10 16:06:48', NULL, NULL, ''),
(1069, '\0', 0, '8bab687b-39c8-44ae-8aa5-e4e4255a953b', 'TYE', NULL, NULL, '2012-11-09 22:09:14', 1066, NULL, '\0'),
(1076, '', 0, '91a03346-79e3-48a3-9a87-ef098b6bca96', 'log 1', NULL, NULL, '2012-11-10 16:01:17', NULL, NULL, '\0'),
(1078, '\0', 0, '9be14cc0-7b81-4045-971b-f7465d770783', 'Logs', NULL, NULL, '2012-11-10 16:01:13', NULL, NULL, ''),
(1067, '\0', 0, 'a22e5d76-7e6b-4dc4-8412-0761629f5ef6', 'Forside', NULL, NULL, '2012-11-09 22:09:39', 1066, NULL, '\0'),
(1060, '\0', 0, 'a42426d9-c409-484a-b7f9-ed7e85661306', 'Vestibulærtræning', NULL, NULL, '2012-11-09 20:02:36', NULL, NULL, '\0'),
(1060, '', 0, 'a84ebf5e-7a50-4fed-8eb7-c75622b4cd87', 'Vestibulærtræning', NULL, NULL, '2012-11-10 07:11:18', NULL, NULL, '\0'),
(1076, '\0', 0, 'ab694aa2-bf3e-4c8b-b0b4-177dbfa04fd1', 'log 1', NULL, NULL, '2012-11-10 16:01:17', NULL, NULL, ''),
(1062, '', 0, 'ad9fbc13-e861-4441-b27f-e6e617fcfb16', 'Start', NULL, NULL, '2012-11-10 16:06:48', NULL, NULL, '\0'),
(1069, '', 0, 'bd1ef6c4-66b3-4c22-924f-b4bc5c1c9574', 'TYE', NULL, NULL, '2012-11-09 22:10:27', 1066, NULL, '\0'),
(1069, '\0', 0, 'bdbb98d8-041e-470e-b1ec-f724c6a82400', 'TYE', NULL, NULL, '2012-11-09 22:10:27', 1066, NULL, ''),
(1062, '\0', 0, 'c3242cd5-7053-4a7a-89a0-a5e23f53e459', 'Start', NULL, NULL, '2012-11-10 16:02:57', NULL, NULL, '\0'),
(1059, '\0', 0, 'e0b45beb-3a6d-4574-81ea-bd67c7498887', 'Exercises', NULL, NULL, '2012-11-09 20:00:37', NULL, NULL, ''),
(1067, '\0', 0, 'e1946700-041b-4f62-81a3-e9d106d602d8', 'Home Dk', NULL, NULL, '2012-11-09 22:02:52', 1066, NULL, '\0'),
(1078, '', 0, 'e2bfc460-a3b8-4112-97e8-340cc9c9ea2f', 'Logs', NULL, NULL, '2012-11-10 16:01:13', NULL, NULL, '\0'),
(1068, '\0', 0, 'e77253ce-4b4c-493f-94c7-327b0c2bb9a3', 'Test dine øjne', NULL, NULL, '2012-11-09 22:10:36', 1066, NULL, '\0'),
(1069, '\0', 0, 'ee5733f9-e2a0-49e4-ad1c-517f7790027e', 'TYE', NULL, NULL, '2012-11-09 22:10:27', 1066, NULL, '\0');

-- --------------------------------------------------------

--
-- Table structure for table `CMSDOCUMENTTYPE`
--

CREATE TABLE IF NOT EXISTS `CMSDOCUMENTTYPE` (
  `CONTENTTYPENODEID` int(11) NOT NULL,
  `TEMPLATENODEID` int(11) NOT NULL,
  `ISDEFAULT` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`CONTENTTYPENODEID`,`TEMPLATENODEID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci;

--
-- Dumping data for table `CMSDOCUMENTTYPE`
--

INSERT INTO `CMSDOCUMENTTYPE` (`CONTENTTYPENODEID`, `TEMPLATENODEID`, `ISDEFAULT`) VALUES
(1063, 1066, ''),
(1065, 1066, '');

-- --------------------------------------------------------

--
-- Table structure for table `CMSLANGUAGETEXT`
--

CREATE TABLE IF NOT EXISTS `CMSLANGUAGETEXT` (
  `PK` int(11) NOT NULL AUTO_INCREMENT,
  `LANGUAGEID` int(11) NOT NULL,
  `UNIQUEID` char(36) COLLATE latin1_danish_ci NOT NULL,
  `VALUE` varchar(1000) CHARACTER SET utf8 NOT NULL,
  PRIMARY KEY (`PK`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=6 ;

--
-- Dumping data for table `CMSLANGUAGETEXT`
--

INSERT INTO `CMSLANGUAGETEXT` (`PK`, `LANGUAGEID`, `UNIQUEID`, `VALUE`) VALUES
(1, 0, 'fbcbf77d-2ca5-491c-866f-0f13bb749182', ''),
(2, 1, 'fbcbf77d-2ca5-491c-866f-0f13bb749182', 'Important'),
(3, 2, 'fbcbf77d-2ca5-491c-866f-0f13bb749182', 'Vigtigt'),
(4, 3, 'fbcbf77d-2ca5-491c-866f-0f13bb749182', 'Vigtigt'),
(5, 4, 'fbcbf77d-2ca5-491c-866f-0f13bb749182', 'Vergnugung');

-- --------------------------------------------------------

--
-- Table structure for table `CMSMACRO`
--

CREATE TABLE IF NOT EXISTS `CMSMACRO` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `MACROUSEINEDITOR` bit(1) NOT NULL DEFAULT b'0',
  `MACROREFRESHRATE` int(11) NOT NULL DEFAULT '0',
  `MACROALIAS` varchar(255) CHARACTER SET utf8 NOT NULL,
  `MACRONAME` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `MACROSCRIPTTYPE` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `MACROSCRIPTASSEMBLY` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `MACROXSLT` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `MACROCACHEBYPAGE` bit(1) NOT NULL DEFAULT b'1',
  `MACROCACHEPERSONALIZED` bit(1) NOT NULL DEFAULT b'0',
  `MACRODONTRENDER` bit(1) NOT NULL DEFAULT b'0',
  `MACROPYTHON` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=3 ;

--
-- Dumping data for table `CMSMACRO`
--

INSERT INTO `CMSMACRO` (`ID`, `MACROUSEINEDITOR`, `MACROREFRESHRATE`, `MACROALIAS`, `MACRONAME`, `MACROSCRIPTTYPE`, `MACROSCRIPTASSEMBLY`, `MACROXSLT`, `MACROCACHEBYPAGE`, `MACROCACHEPERSONALIZED`, `MACRODONTRENDER`, `MACROPYTHON`) VALUES
(1, '\0', 0, 'Navigation', 'Navigation', NULL, NULL, 'Navigation.xslt', '', '\0', '\0', NULL),
(2, '', 0, 'testing', 'testing', 'usercontrols/testing.ascx', '', '', '', '\0', '\0', '');

-- --------------------------------------------------------

--
-- Table structure for table `CMSMACROPROPERTY`
--

CREATE TABLE IF NOT EXISTS `CMSMACROPROPERTY` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `MACROPROPERTYHIDDEN` bit(1) NOT NULL DEFAULT b'0',
  `MACROPROPERTYTYPE` smallint(6) NOT NULL,
  `MACRO` int(11) NOT NULL,
  `MACROPROPERTYSORTORDER` tinyint(4) NOT NULL DEFAULT '0',
  `MACROPROPERTYALIAS` varchar(50) CHARACTER SET utf8 NOT NULL,
  `MACROPROPERTYNAME` varchar(255) CHARACTER SET utf8 NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `MACROPROPERTYTYPE` (`MACROPROPERTYTYPE`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `CMSMACROPROPERTYTYPE`
--

CREATE TABLE IF NOT EXISTS `CMSMACROPROPERTYTYPE` (
  `ID` smallint(6) NOT NULL AUTO_INCREMENT,
  `MACROPROPERTYTYPEALIAS` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `MACROPROPERTYTYPERENDERASSEMBLY` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `MACROPROPERTYTYPERENDERTYPE` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `MACROPROPERTYTYPEBASETYPE` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=26 ;

--
-- Dumping data for table `CMSMACROPROPERTYTYPE`
--

INSERT INTO `CMSMACROPROPERTYTYPE` (`ID`, `MACROPROPERTYTYPEALIAS`, `MACROPROPERTYTYPERENDERASSEMBLY`, `MACROPROPERTYTYPERENDERTYPE`, `MACROPROPERTYTYPEBASETYPE`) VALUES
(3, 'mediaCurrent', 'umbraco.macroRenderings', 'media', 'Int32'),
(4, 'contentSubs', 'umbraco.macroRenderings', 'content', 'Int32'),
(5, 'contentRandom', 'umbraco.macroRenderings', 'content', 'Int32'),
(6, 'contentPicker', 'umbraco.macroRenderings', 'content', 'Int32'),
(13, 'number', 'umbraco.macroRenderings', 'numeric', 'Int32'),
(14, 'bool', 'umbraco.macroRenderings', 'yesNo', 'Boolean'),
(16, 'text', 'umbraco.macroRenderings', 'text', 'String'),
(17, 'contentTree', 'umbraco.macroRenderings', 'content', 'Int32'),
(18, 'contentType', 'umbraco.macroRenderings', 'contentTypeSingle', 'Int32'),
(19, 'contentTypeMultiple', 'umbraco.macroRenderings', 'contentTypeMultiple', 'Int32'),
(20, 'contentAll', 'umbraco.macroRenderings', 'content', 'Int32'),
(21, 'tabPicker', 'umbraco.macroRenderings', 'tabPicker', 'String'),
(22, 'tabPickerMultiple', 'umbraco.macroRenderings', 'tabPickerMultiple', 'String'),
(23, 'propertyTypePicker', 'umbraco.macroRenderings', 'propertyTypePicker', 'String'),
(24, 'propertyTypePickerMultiple', 'umbraco.macroRenderings', 'propertyTypePickerMultiple', 'String'),
(25, 'textMultiLine', 'umbraco.macroRenderings', 'textMultiple', 'String');

-- --------------------------------------------------------

--
-- Table structure for table `CMSMEMBER`
--

CREATE TABLE IF NOT EXISTS `CMSMEMBER` (
  `NODEID` int(11) NOT NULL,
  `EMAIL` varchar(1000) CHARACTER SET utf8 NOT NULL DEFAULT '',
  `LOGINNAME` varchar(1000) CHARACTER SET utf8 NOT NULL DEFAULT '',
  `PASSWORD` varchar(1000) CHARACTER SET utf8 NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci;

--
-- Dumping data for table `CMSMEMBER`
--

INSERT INTO `CMSMEMBER` (`NODEID`, `EMAIL`, `LOGINNAME`, `PASSWORD`) VALUES
(1053, 'contact@monosolutions.dk', 'monosolutions', 'rQwmzhw83dAEfuuKWZo9E04bClI='),
(1071, 'aa@aa.com', 'aa', '5cWVHBP6rOd3kzz1a95jzv24MCc=');

-- --------------------------------------------------------

--
-- Table structure for table `CMSMEMBER2MEMBERGROUP`
--

CREATE TABLE IF NOT EXISTS `CMSMEMBER2MEMBERGROUP` (
  `MEMBER` int(11) NOT NULL,
  `MEMBERGROUP` int(11) NOT NULL,
  PRIMARY KEY (`MEMBER`,`MEMBERGROUP`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci;

--
-- Dumping data for table `CMSMEMBER2MEMBERGROUP`
--

INSERT INTO `CMSMEMBER2MEMBERGROUP` (`MEMBER`, `MEMBERGROUP`) VALUES
(1053, 1051),
(1071, 1051);

-- --------------------------------------------------------

--
-- Table structure for table `CMSMEMBERTYPE`
--

CREATE TABLE IF NOT EXISTS `CMSMEMBERTYPE` (
  `PK` int(11) NOT NULL AUTO_INCREMENT,
  `NODEID` int(11) NOT NULL,
  `PROPERTYTYPEID` int(11) NOT NULL,
  `MEMBERCANEDIT` bit(1) NOT NULL DEFAULT b'0',
  `VIEWONPROFILE` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`PK`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `CMSPREVIEWXML`
--

CREATE TABLE IF NOT EXISTS `CMSPREVIEWXML` (
  `NODEID` int(11) NOT NULL,
  `VERSIONID` char(36) COLLATE latin1_danish_ci NOT NULL,
  `TIMESTAMP` datetime NOT NULL,
  `XML` longtext COLLATE latin1_danish_ci NOT NULL,
  PRIMARY KEY (`NODEID`,`VERSIONID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci;

--
-- Dumping data for table `CMSPREVIEWXML`
--

INSERT INTO `CMSPREVIEWXML` (`NODEID`, `VERSIONID`, `TIMESTAMP`, `XML`) VALUES
(1059, '2f16dfa5-751a-4c40-83aa-1f1f640b5bf9', '2012-11-09 17:36:58', '<ExerciseFolder id="1059" parentID="-1" level="1" writerID="0" creatorID="0" nodeType="1058" template="0" sortOrder="0" createDate="2012-11-09T17:36:58" updateDate="2012-11-09T17:36:58" nodeName="Exercises" urlName="exercises" writerName="monosolutions" creatorName="monosolutions" path="-1,1059" isDoc="" />'),
(1060, '1f071181-5511-40ea-9be7-e44a86657f5a', '2012-11-09 20:28:02', '<Exercise id="1060" parentID="1059" level="2" writerID="0" creatorID="0" nodeType="1056" template="0" sortOrder="0" createDate="2012-11-09T20:28:02" updateDate="2012-11-09T20:28:02" nodeName="Vestibulærtræning" urlName="vestibulaertraening" writerName="monosolutions" creatorName="monosolutions" path="-1,1059,1060" isDoc=""><type><![CDATA[]]></type></Exercise>'),
(1060, '2ea61431-3272-40df-ba03-443f02a5da2a', '2012-11-09 21:02:35', '<Exercise id="1060" parentID="1059" level="2" writerID="0" creatorID="0" nodeType="1056" template="0" sortOrder="0" createDate="2012-11-09T20:28:02" updateDate="2012-11-09T21:01:55" nodeName="Vestibulærtræning" urlName="vestibulaertraening" writerName="monosolutions" creatorName="monosolutions" path="-1,1059,1060" isDoc=""><type>6</type><nameDk>Vestibulærtræning</nameDk><nameDe></nameDe><nameGb></nameGb><nameNo></nameNo><textDk><![CDATA[<p><span>Formål:</span></p>\r\n<ol>\r\n<li>At få en bedre balance.</li>\r\n<li>At blive bedre til at klare svimmelhed.</li>\r\n</ol>]]></textDk><textDe><![CDATA[<p>asdfasdf</p>]]></textDe><textGb><![CDATA[]]></textGb><textNo><![CDATA[]]></textNo></Exercise>'),
(1060, '6606919f-5cb4-497f-8161-c9be3d4707dd', '2012-11-09 23:15:46', '<Exercise id="1060" parentID="1059" level="2" writerID="0" creatorID="0" nodeType="1056" template="0" sortOrder="0" createDate="2012-11-09T20:28:02" updateDate="2012-11-09T21:02:35" nodeName="Vestibulærtræning" urlName="vestibulaertraening" writerName="monosolutions" creatorName="monosolutions" path="-1,1059,1060" isDoc=""><type>6</type><nameDk>Vestibulærtræning</nameDk><nameDe></nameDe><nameGb></nameGb><nameNo></nameNo><textDk><![CDATA[<p><span>Formål:</span></p>\r\n<ol>\r\n<li>At få en bedre balance.</li>\r\n<li>At blive <strong>bedre</strong> til at klare svimmelhed.</li>\r\n</ol>]]></textDk><textDe><![CDATA[<p><strong>asdfasdf</strong></p>]]></textDe><textGb><![CDATA[]]></textGb><textNo><![CDATA[]]></textNo></Exercise>'),
(1060, 'a42426d9-c409-484a-b7f9-ed7e85661306', '2012-11-09 21:01:55', '<Exercise id="1060" parentID="1059" level="2" writerID="0" creatorID="0" nodeType="1056" template="0" sortOrder="0" createDate="2012-11-09T20:28:02" updateDate="2012-11-09T21:00:36" nodeName="Vestibulærtræning" urlName="vestibulaertraening" writerName="monosolutions" creatorName="monosolutions" path="-1,1059,1060" isDoc=""><type>6</type><nameDk>Vestibulærtræning</nameDk><nameDe></nameDe><nameGb></nameGb><nameNo></nameNo><textDk><![CDATA[<p><span>Formål:</span></p>\r\n<ol>\r\n<li>At få en bedre balance.</li>\r\n<li>At blive bedre til at klare svimmelhed.</li>\r\n</ol>]]></textDk><textDe><![CDATA[]]></textDe><textGb><![CDATA[]]></textGb><textNo><![CDATA[]]></textNo></Exercise>'),
(1060, 'a84ebf5e-7a50-4fed-8eb7-c75622b4cd87', '2012-11-10 08:11:18', '<Exercise id="1060" parentID="1059" level="2" writerID="0" creatorID="0" nodeType="1056" template="0" sortOrder="0" createDate="2012-11-09T20:28:02" updateDate="2012-11-09T23:15:46" nodeName="Vestibulærtræning" urlName="vestibulaertraening" writerName="monosolutions" creatorName="monosolutions" path="-1,1059,1060" isDoc=""><type>6</type><nameDk>Vestibulærtræning</nameDk><nameDe></nameDe><nameGb></nameGb><nameNo></nameNo><textDk><![CDATA[<p><span>Formål:</span></p>\r\n<ol>\r\n<li>At få en bedre balance.</li>\r\n<li>At blive <strong>bedre</strong> til at klare svimmelhed.</li>\r\n</ol>]]></textDk><textDe><![CDATA[<p><strong>asdfasdf</strong></p>]]></textDe><textGb><![CDATA[]]></textGb><textNo><![CDATA[]]></textNo></Exercise>'),
(1062, '052bd737-216b-48e1-b09f-69189d6641f1', '2012-11-10 17:02:57', '<ExerciseInfo id="1062" parentID="1060" level="3" writerID="0" creatorID="0" nodeType="1057" template="0" sortOrder="0" createDate="2012-11-09T20:56:34" updateDate="2012-11-09T21:00:36" nodeName="Start" urlName="start" writerName="monosolutions" creatorName="monosolutions" path="-1,1059,1060,1062" isDoc=""><headingDk>Intro</headingDk><headingDe>Intro</headingDe><headingGb>Intro</headingGb><headingNo>Intro</headingNo><textDk><![CDATA[<ol>\r\n<li>Denne øvelse er opdelt i 7 forskellige trin, der træner hver deres del af labyrintsansen. Læs evt. mere om labyrintsansen under balancen.</li>\r\n<li>Har man problemer med balancen skal man lave denne øvelse i små portioner, da de kan fremkalde svimmelhed og ubehag.</li>\r\n<li>Det er ikke sikkert, at man har problemer med alle delene. Hvis man kan lave to af trinene uden problemer, skal man bare gå videre til næste.</li>\r\n</ol>]]></textDk><textDe><![CDATA[]]></textDe><textGb><![CDATA[]]></textGb><textNo><![CDATA[]]></textNo></ExerciseInfo>'),
(1062, 'ad9fbc13-e861-4441-b27f-e6e617fcfb16', '2012-11-10 17:06:47', '<ExerciseInfo id="1062" parentID="1060" level="3" writerID="0" creatorID="0" nodeType="1057" template="0" sortOrder="0" createDate="2012-11-09T20:56:34" updateDate="2012-11-10T17:02:57" nodeName="Start" urlName="start" writerName="monosolutions" creatorName="monosolutions" path="-1,1059,1060,1062" isDoc=""><headingDk>Intro</headingDk><headingDe>Intro</headingDe><headingGb>Intro</headingGb><headingNo>Intro</headingNo><textDk><![CDATA[<ol>\r\n<li>Denne øvelse er opdelt i 7 forskellige trin, der træner hver deres del af labyrintsansen. Læs evt. mere om labyrintsansen under balancen.</li>\r\n<li>Har man problemer med balancen skal man lave denne øvelse i små portioner, da de kan fremkalde svimmelhed og ubehag.</li>\r\n<li>Det er ikke sikkert, at man har problemer med alle delene. Hvis man kan lave to af trinene uden problemer, skal man bare gå videre til næste.</li>\r\n</ol>]]></textDk><textDe><![CDATA[]]></textDe><textGb><![CDATA[]]></textGb><textNo><![CDATA[]]></textNo></ExerciseInfo>'),
(1062, 'c3242cd5-7053-4a7a-89a0-a5e23f53e459', '2012-11-09 20:56:34', '<ExerciseInfo id="1062" parentID="1060" level="3" writerID="0" creatorID="0" nodeType="1057" template="0" sortOrder="0" createDate="2012-11-09T20:56:34" updateDate="2012-11-09T20:56:34" nodeName="Start" urlName="start" writerName="monosolutions" creatorName="monosolutions" path="-1,1059,1060,1062" isDoc=""><textDk><![CDATA[]]></textDk><textDe><![CDATA[]]></textDe><textGb><![CDATA[]]></textGb><textNo><![CDATA[]]></textNo></ExerciseInfo>'),
(1067, '6b734643-6548-4860-8504-abd705d055d9', '2012-11-09 23:09:39', '<master id="1067" parentID="-1" level="1" writerID="0" creatorID="0" nodeType="1063" template="1066" sortOrder="0" createDate="2012-11-09T21:13:41" updateDate="2012-11-09T23:02:51" nodeName="Forside" urlName="forside" writerName="monosolutions" creatorName="monosolutions" path="-1,1067" isDoc=""><seoPageTitle></seoPageTitle><seoKeywords></seoKeywords><seoDescription></seoDescription><bodyText><![CDATA[<p>text</p>]]></bodyText></master>'),
(1067, 'a22e5d76-7e6b-4dc4-8412-0761629f5ef6', '2012-11-09 23:02:51', '<master id="1067" parentID="-1" level="1" writerID="0" creatorID="0" nodeType="1063" template="1066" sortOrder="0" createDate="2012-11-09T21:13:41" updateDate="2012-11-09T22:56:54" nodeName="Forside" urlName="forside" writerName="monosolutions" creatorName="monosolutions" path="-1,1067" isDoc=""><seoPageTitle></seoPageTitle><seoKeywords></seoKeywords><seoDescription></seoDescription><bodyText><![CDATA[]]></bodyText></master>'),
(1067, 'e1946700-041b-4f62-81a3-e9d106d602d8', '2012-11-09 22:56:54', '<master id="1067" parentID="-1" level="1" writerID="0" creatorID="0" nodeType="1063" template="1066" sortOrder="0" createDate="2012-11-09T21:13:41" updateDate="2012-11-09T21:13:41" nodeName="Home Dk" urlName="home-dk" writerName="monosolutions" creatorName="monosolutions" path="-1,1067" isDoc="" />'),
(1068, '2aa77de6-dc62-4dea-9752-e2b0795635b2', '2012-11-10 15:39:51', '<page id="1068" parentID="1067" level="2" writerID="0" creatorID="0" nodeType="1065" template="1066" sortOrder="0" createDate="2012-11-09T22:59:35" updateDate="2012-11-09T23:10:35" nodeName="Test dine øjne" urlName="test-dine-oejne" writerName="monosolutions" creatorName="monosolutions" path="-1,1067,1068" isDoc=""><seoPageTitle></seoPageTitle><seoKeywords></seoKeywords><seoDescription></seoDescription><bodyText><![CDATA[<p>haloooo!</p>\r\n<p> </p>\r\n<?UMBRACO_MACRO macroAlias="testing" />\r\n<p> </p>]]></bodyText></page>'),
(1068, '38db764b-695d-4d5c-be69-49fe6e112328', '2012-11-09 23:10:35', '<page id="1068" parentID="1067" level="2" writerID="0" creatorID="0" nodeType="1065" template="1066" sortOrder="0" createDate="2012-11-09T22:59:35" updateDate="2012-11-09T23:02:05" nodeName="Test dine øjne" urlName="test-dine-oejne" writerName="monosolutions" creatorName="monosolutions" path="-1,1067,1068" isDoc=""><seoPageTitle></seoPageTitle><seoKeywords></seoKeywords><seoDescription></seoDescription><bodyText><![CDATA[<p>haloooo!</p>]]></bodyText></page>'),
(1068, 'e77253ce-4b4c-493f-94c7-327b0c2bb9a3', '2012-11-09 23:02:05', '<page id="1068" parentID="1067" level="2" writerID="0" creatorID="0" nodeType="1065" template="1066" sortOrder="0" createDate="2012-11-09T22:59:35" updateDate="2012-11-09T22:59:35" nodeName="Test dine øjne" urlName="test-dine-oejne" writerName="monosolutions" creatorName="monosolutions" path="-1,1067,1068" isDoc=""><seoPageTitle></seoPageTitle><seoKeywords></seoKeywords><seoDescription></seoDescription><bodyText><![CDATA[]]></bodyText></page>'),
(1069, '36ef08f8-adea-4165-b19c-20c9523bd6a0', '2012-11-09 23:09:13', '<page id="1069" parentID="1067" level="2" writerID="0" creatorID="0" nodeType="1065" template="1066" sortOrder="0" createDate="2012-11-09T23:08:03" updateDate="2012-11-09T23:08:12" nodeName="TYE" urlName="tye" writerName="monosolutions" creatorName="monosolutions" path="-1,1067,1069" isDoc=""><seoPageTitle></seoPageTitle><seoKeywords></seoKeywords><seoDescription></seoDescription><bodyText><![CDATA[]]></bodyText></page>'),
(1069, '8bab687b-39c8-44ae-8aa5-e4e4255a953b', '2012-11-09 23:08:12', '<page id="1069" parentID="1067" level="2" writerID="0" creatorID="0" nodeType="1065" template="1066" sortOrder="0" createDate="2012-11-09T23:08:03" updateDate="2012-11-09T23:08:03" nodeName="TYE" urlName="tye" writerName="monosolutions" creatorName="monosolutions" path="-1,1067,1069" isDoc=""><seoPageTitle></seoPageTitle><seoKeywords></seoKeywords><seoDescription></seoDescription><bodyText><![CDATA[]]></bodyText></page>'),
(1069, 'bd1ef6c4-66b3-4c22-924f-b4bc5c1c9574', '2012-11-09 23:10:26', '<page id="1069" parentID="1067" level="2" writerID="0" creatorID="0" nodeType="1065" template="1066" sortOrder="0" createDate="2012-11-09T23:08:03" updateDate="2012-11-09T23:09:33" nodeName="TYE" urlName="tye" writerName="monosolutions" creatorName="monosolutions" path="-1,1067,1069" isDoc=""><seoPageTitle></seoPageTitle><seoKeywords></seoKeywords><seoDescription></seoDescription><bodyText><![CDATA[<p>snaps!</p>]]></bodyText></page>'),
(1069, 'ee5733f9-e2a0-49e4-ad1c-517f7790027e', '2012-11-09 23:09:33', '<page id="1069" parentID="1067" level="2" writerID="0" creatorID="0" nodeType="1065" template="1066" sortOrder="0" createDate="2012-11-09T23:08:03" updateDate="2012-11-09T23:09:13" nodeName="TYE" urlName="tye" writerName="monosolutions" creatorName="monosolutions" path="-1,1067,1069" isDoc=""><seoPageTitle></seoPageTitle><seoKeywords></seoKeywords><seoDescription></seoDescription><bodyText><![CDATA[<p>snaps!</p>]]></bodyText></page>'),
(1076, '91a03346-79e3-48a3-9a87-ef098b6bca96', '2012-11-10 17:01:16', '<ExerciseLog id="1076" parentID="1078" level="2" writerID="0" creatorID="0" nodeType="1074" template="0" sortOrder="0" createDate="2012-11-10T16:52:08" updateDate="2012-11-10T16:52:08" nodeName="log 1" urlName="log-1" writerName="monosolutions" creatorName="monosolutions" path="-1,1078,1076" isDoc=""><exercise>1060</exercise><member>1053</member><timeStart>2012-11-10T04:20:00</timeStart><timeEnd>2012-11-11T11:41:00</timeEnd><note><![CDATA[]]></note><score>1</score></ExerciseLog>'),
(1078, 'e2bfc460-a3b8-4112-97e8-340cc9c9ea2f', '2012-11-10 17:01:13', '<ExerciseLogFolder id="1078" parentID="-1" level="1" writerID="0" creatorID="0" nodeType="1077" template="0" sortOrder="0" createDate="2012-11-10T17:00:54" updateDate="2012-11-10T17:00:54" nodeName="Logs" urlName="logs" writerName="monosolutions" creatorName="monosolutions" path="-1,1078" isDoc="" />');

-- --------------------------------------------------------

--
-- Table structure for table `CMSPROPERTYDATA`
--

CREATE TABLE IF NOT EXISTS `CMSPROPERTYDATA` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `CONTENTNODEID` int(11) NOT NULL,
  `VERSIONID` char(36) COLLATE latin1_danish_ci DEFAULT NULL,
  `PROPERTYTYPEID` int(11) NOT NULL,
  `DATAINT` int(11) DEFAULT NULL,
  `DATADATE` datetime DEFAULT NULL,
  `DATANVARCHAR` varchar(500) CHARACTER SET utf8 DEFAULT NULL,
  `DATANTEXT` longtext COLLATE latin1_danish_ci,
  PRIMARY KEY (`ID`),
  KEY `IX_CMSPROPERTYDATA_1` (`CONTENTNODEID`),
  KEY `IX_CMSPROPERTYDATA_2` (`VERSIONID`),
  KEY `IX_CMSPROPERTYDATA_3` (`PROPERTYTYPEID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=152 ;

--
-- Dumping data for table `CMSPROPERTYDATA`
--

INSERT INTO `CMSPROPERTYDATA` (`ID`, `CONTENTNODEID`, `VERSIONID`, `PROPERTYTYPEID`, `DATAINT`, `DATADATE`, `DATANVARCHAR`, `DATANTEXT`) VALUES
(1, 1060, '1f071181-5511-40ea-9be7-e44a86657f5a', 28, NULL, NULL, NULL, NULL),
(2, 1060, '1f071181-5511-40ea-9be7-e44a86657f5a', 34, NULL, NULL, NULL, NULL),
(3, 1060, '1f071181-5511-40ea-9be7-e44a86657f5a', 35, NULL, NULL, NULL, NULL),
(4, 1060, '1f071181-5511-40ea-9be7-e44a86657f5a', 36, NULL, NULL, NULL, NULL),
(5, 1060, '1f071181-5511-40ea-9be7-e44a86657f5a', 37, NULL, NULL, NULL, NULL),
(6, 1060, '1f071181-5511-40ea-9be7-e44a86657f5a', 38, NULL, NULL, NULL, NULL),
(7, 1060, '1f071181-5511-40ea-9be7-e44a86657f5a', 39, NULL, NULL, NULL, NULL),
(8, 1060, '1f071181-5511-40ea-9be7-e44a86657f5a', 40, NULL, NULL, NULL, NULL),
(9, 1060, '1f071181-5511-40ea-9be7-e44a86657f5a', 41, NULL, NULL, NULL, NULL),
(10, 1062, 'c3242cd5-7053-4a7a-89a0-a5e23f53e459', 30, NULL, NULL, NULL, NULL),
(11, 1062, 'c3242cd5-7053-4a7a-89a0-a5e23f53e459', 31, NULL, NULL, NULL, NULL),
(12, 1062, 'c3242cd5-7053-4a7a-89a0-a5e23f53e459', 32, NULL, NULL, NULL, NULL),
(13, 1062, 'c3242cd5-7053-4a7a-89a0-a5e23f53e459', 33, NULL, NULL, NULL, NULL),
(14, 1062, 'c3242cd5-7053-4a7a-89a0-a5e23f53e459', 42, NULL, NULL, NULL, NULL),
(15, 1062, 'c3242cd5-7053-4a7a-89a0-a5e23f53e459', 43, NULL, NULL, NULL, NULL),
(16, 1062, 'c3242cd5-7053-4a7a-89a0-a5e23f53e459', 44, NULL, NULL, NULL, NULL),
(17, 1062, 'c3242cd5-7053-4a7a-89a0-a5e23f53e459', 45, NULL, NULL, NULL, NULL),
(18, 1060, 'a42426d9-c409-484a-b7f9-ed7e85661306', 28, NULL, NULL, '6', NULL),
(19, 1060, 'a42426d9-c409-484a-b7f9-ed7e85661306', 34, NULL, NULL, 'Vestibulærtræning', NULL),
(20, 1060, 'a42426d9-c409-484a-b7f9-ed7e85661306', 35, NULL, NULL, '', NULL),
(21, 1060, 'a42426d9-c409-484a-b7f9-ed7e85661306', 36, NULL, NULL, '', NULL),
(22, 1060, 'a42426d9-c409-484a-b7f9-ed7e85661306', 37, NULL, NULL, '', NULL),
(23, 1060, 'a42426d9-c409-484a-b7f9-ed7e85661306', 38, NULL, NULL, NULL, '<p><span>Formål:</span></p>\r\n<ol>\r\n<li>At få en bedre balance.</li>\r\n<li>At blive bedre til at klare svimmelhed.</li>\r\n</ol>'),
(24, 1060, 'a42426d9-c409-484a-b7f9-ed7e85661306', 39, NULL, NULL, NULL, ''),
(25, 1060, 'a42426d9-c409-484a-b7f9-ed7e85661306', 40, NULL, NULL, NULL, ''),
(26, 1060, 'a42426d9-c409-484a-b7f9-ed7e85661306', 41, NULL, NULL, NULL, ''),
(27, 1062, '052bd737-216b-48e1-b09f-69189d6641f1', 42, NULL, NULL, 'Intro', NULL),
(28, 1062, '052bd737-216b-48e1-b09f-69189d6641f1', 43, NULL, NULL, 'Intro', NULL),
(29, 1062, '052bd737-216b-48e1-b09f-69189d6641f1', 44, NULL, NULL, 'Intro', NULL),
(30, 1062, '052bd737-216b-48e1-b09f-69189d6641f1', 45, NULL, NULL, 'Intro', NULL),
(31, 1062, '052bd737-216b-48e1-b09f-69189d6641f1', 30, NULL, NULL, NULL, '<ol>\r\n<li>Denne øvelse er opdelt i 7 forskellige trin, der træner hver deres del af labyrintsansen. Læs evt. mere om labyrintsansen under balancen.</li>\r\n<li>Har man problemer med balancen skal man lave denne øvelse i små portioner, da de kan fremkalde svimmelhed og ubehag.</li>\r\n<li>Det er ikke sikkert, at man har problemer med alle delene. Hvis man kan lave to af trinene uden problemer, skal man bare gå videre til næste.</li>\r\n</ol>'),
(32, 1062, '052bd737-216b-48e1-b09f-69189d6641f1', 31, NULL, NULL, NULL, ''),
(33, 1062, '052bd737-216b-48e1-b09f-69189d6641f1', 32, NULL, NULL, NULL, ''),
(34, 1062, '052bd737-216b-48e1-b09f-69189d6641f1', 33, NULL, NULL, NULL, ''),
(35, 1060, '2ea61431-3272-40df-ba03-443f02a5da2a', 28, NULL, NULL, '6', NULL),
(36, 1060, '2ea61431-3272-40df-ba03-443f02a5da2a', 34, NULL, NULL, 'Vestibulærtræning', NULL),
(37, 1060, '2ea61431-3272-40df-ba03-443f02a5da2a', 35, NULL, NULL, '', NULL),
(38, 1060, '2ea61431-3272-40df-ba03-443f02a5da2a', 36, NULL, NULL, '', NULL),
(39, 1060, '2ea61431-3272-40df-ba03-443f02a5da2a', 37, NULL, NULL, '', NULL),
(40, 1060, '2ea61431-3272-40df-ba03-443f02a5da2a', 38, NULL, NULL, NULL, '<p><span>Formål:</span></p>\r\n<ol>\r\n<li>At få en bedre balance.</li>\r\n<li>At blive bedre til at klare svimmelhed.</li>\r\n</ol>'),
(41, 1060, '2ea61431-3272-40df-ba03-443f02a5da2a', 39, NULL, NULL, NULL, '<p>asdfasdf</p>'),
(42, 1060, '2ea61431-3272-40df-ba03-443f02a5da2a', 40, NULL, NULL, NULL, ''),
(43, 1060, '2ea61431-3272-40df-ba03-443f02a5da2a', 41, NULL, NULL, NULL, ''),
(44, 1060, '6606919f-5cb4-497f-8161-c9be3d4707dd', 28, NULL, NULL, '6', NULL),
(45, 1060, '6606919f-5cb4-497f-8161-c9be3d4707dd', 34, NULL, NULL, 'Vestibulærtræning', NULL),
(46, 1060, '6606919f-5cb4-497f-8161-c9be3d4707dd', 35, NULL, NULL, '', NULL),
(47, 1060, '6606919f-5cb4-497f-8161-c9be3d4707dd', 36, NULL, NULL, '', NULL),
(48, 1060, '6606919f-5cb4-497f-8161-c9be3d4707dd', 37, NULL, NULL, '', NULL),
(49, 1060, '6606919f-5cb4-497f-8161-c9be3d4707dd', 38, NULL, NULL, NULL, '<p><span>Formål:</span></p>\r\n<ol>\r\n<li>At få en bedre balance.</li>\r\n<li>At blive <strong>bedre</strong> til at klare svimmelhed.</li>\r\n</ol>'),
(50, 1060, '6606919f-5cb4-497f-8161-c9be3d4707dd', 39, NULL, NULL, NULL, '<p><strong>asdfasdf</strong></p>'),
(51, 1060, '6606919f-5cb4-497f-8161-c9be3d4707dd', 40, NULL, NULL, NULL, ''),
(52, 1060, '6606919f-5cb4-497f-8161-c9be3d4707dd', 41, NULL, NULL, NULL, ''),
(53, 1067, 'e1946700-041b-4f62-81a3-e9d106d602d8', 46, NULL, NULL, NULL, NULL),
(54, 1067, 'a22e5d76-7e6b-4dc4-8412-0761629f5ef6', 46, NULL, NULL, '', NULL),
(56, 1068, 'e77253ce-4b4c-493f-94c7-327b0c2bb9a3', 46, NULL, NULL, '', NULL),
(57, 1067, 'e1946700-041b-4f62-81a3-e9d106d602d8', 47, NULL, NULL, NULL, NULL),
(58, 1067, 'a22e5d76-7e6b-4dc4-8412-0761629f5ef6', 47, NULL, NULL, '', NULL),
(60, 1068, 'e77253ce-4b4c-493f-94c7-327b0c2bb9a3', 47, NULL, NULL, '', NULL),
(61, 1067, 'e1946700-041b-4f62-81a3-e9d106d602d8', 48, NULL, NULL, NULL, NULL),
(62, 1067, 'a22e5d76-7e6b-4dc4-8412-0761629f5ef6', 48, NULL, NULL, '', NULL),
(64, 1068, 'e77253ce-4b4c-493f-94c7-327b0c2bb9a3', 48, NULL, NULL, '', NULL),
(65, 1067, 'e1946700-041b-4f62-81a3-e9d106d602d8', 49, NULL, NULL, NULL, NULL),
(66, 1067, 'a22e5d76-7e6b-4dc4-8412-0761629f5ef6', 49, NULL, NULL, NULL, ''),
(68, 1068, 'e77253ce-4b4c-493f-94c7-327b0c2bb9a3', 49, NULL, NULL, NULL, ''),
(69, 1068, '38db764b-695d-4d5c-be69-49fe6e112328', 46, NULL, NULL, '', NULL),
(70, 1068, '38db764b-695d-4d5c-be69-49fe6e112328', 47, NULL, NULL, '', NULL),
(71, 1068, '38db764b-695d-4d5c-be69-49fe6e112328', 48, NULL, NULL, '', NULL),
(72, 1068, '38db764b-695d-4d5c-be69-49fe6e112328', 49, NULL, NULL, NULL, '<p>haloooo!</p>'),
(73, 1067, '6b734643-6548-4860-8504-abd705d055d9', 46, NULL, NULL, '', NULL),
(74, 1067, '6b734643-6548-4860-8504-abd705d055d9', 47, NULL, NULL, '', NULL),
(75, 1067, '6b734643-6548-4860-8504-abd705d055d9', 48, NULL, NULL, '', NULL),
(76, 1067, '6b734643-6548-4860-8504-abd705d055d9', 49, NULL, NULL, NULL, '<p>text</p>'),
(77, 1069, '8bab687b-39c8-44ae-8aa5-e4e4255a953b', 46, NULL, NULL, '', NULL),
(78, 1069, '8bab687b-39c8-44ae-8aa5-e4e4255a953b', 47, NULL, NULL, '', NULL),
(79, 1069, '8bab687b-39c8-44ae-8aa5-e4e4255a953b', 48, NULL, NULL, '', NULL),
(80, 1069, '8bab687b-39c8-44ae-8aa5-e4e4255a953b', 49, NULL, NULL, NULL, ''),
(81, 1069, '36ef08f8-adea-4165-b19c-20c9523bd6a0', 46, NULL, NULL, '', NULL),
(82, 1069, '36ef08f8-adea-4165-b19c-20c9523bd6a0', 47, NULL, NULL, '', NULL),
(83, 1069, '36ef08f8-adea-4165-b19c-20c9523bd6a0', 48, NULL, NULL, '', NULL),
(84, 1069, '36ef08f8-adea-4165-b19c-20c9523bd6a0', 49, NULL, NULL, NULL, ''),
(85, 1069, 'ee5733f9-e2a0-49e4-ad1c-517f7790027e', 46, NULL, NULL, '', NULL),
(86, 1069, 'ee5733f9-e2a0-49e4-ad1c-517f7790027e', 47, NULL, NULL, '', NULL),
(87, 1069, 'ee5733f9-e2a0-49e4-ad1c-517f7790027e', 48, NULL, NULL, '', NULL),
(88, 1069, 'ee5733f9-e2a0-49e4-ad1c-517f7790027e', 49, NULL, NULL, NULL, '<p>snaps!</p>'),
(89, 1069, 'bd1ef6c4-66b3-4c22-924f-b4bc5c1c9574', 46, NULL, NULL, '', NULL),
(90, 1069, 'bd1ef6c4-66b3-4c22-924f-b4bc5c1c9574', 47, NULL, NULL, '', NULL),
(91, 1069, 'bd1ef6c4-66b3-4c22-924f-b4bc5c1c9574', 48, NULL, NULL, '', NULL),
(92, 1069, 'bd1ef6c4-66b3-4c22-924f-b4bc5c1c9574', 49, NULL, NULL, NULL, '<p>snaps!</p>'),
(93, 1067, '1d48a244-1f86-4acb-b83e-c70d8c79a5ad', 46, NULL, NULL, NULL, NULL),
(94, 1067, '1d48a244-1f86-4acb-b83e-c70d8c79a5ad', 47, NULL, NULL, NULL, NULL),
(95, 1067, '1d48a244-1f86-4acb-b83e-c70d8c79a5ad', 48, NULL, NULL, NULL, NULL),
(96, 1067, '1d48a244-1f86-4acb-b83e-c70d8c79a5ad', 49, NULL, NULL, NULL, '<p>text</p>'),
(97, 1069, 'bdbb98d8-041e-470e-b1ec-f724c6a82400', 46, NULL, NULL, NULL, NULL),
(98, 1069, 'bdbb98d8-041e-470e-b1ec-f724c6a82400', 47, NULL, NULL, NULL, NULL),
(99, 1069, 'bdbb98d8-041e-470e-b1ec-f724c6a82400', 48, NULL, NULL, NULL, NULL),
(100, 1069, 'bdbb98d8-041e-470e-b1ec-f724c6a82400', 49, NULL, NULL, NULL, '<p>snaps!</p>'),
(101, 1068, '2aa77de6-dc62-4dea-9752-e2b0795635b2', 46, NULL, NULL, '', NULL),
(102, 1068, '2aa77de6-dc62-4dea-9752-e2b0795635b2', 47, NULL, NULL, '', NULL),
(103, 1068, '2aa77de6-dc62-4dea-9752-e2b0795635b2', 48, NULL, NULL, '', NULL),
(104, 1068, '2aa77de6-dc62-4dea-9752-e2b0795635b2', 49, NULL, NULL, NULL, '<p>haloooo!</p>\r\n<p> </p>\r\n<?UMBRACO_MACRO macroAlias="testing" />\r\n<p> </p>'),
(105, 1060, 'a84ebf5e-7a50-4fed-8eb7-c75622b4cd87', 28, NULL, NULL, '6', NULL),
(106, 1060, 'a84ebf5e-7a50-4fed-8eb7-c75622b4cd87', 34, NULL, NULL, 'Vestibulærtræning', NULL),
(107, 1060, 'a84ebf5e-7a50-4fed-8eb7-c75622b4cd87', 35, NULL, NULL, '', NULL),
(108, 1060, 'a84ebf5e-7a50-4fed-8eb7-c75622b4cd87', 36, NULL, NULL, '', NULL),
(109, 1060, 'a84ebf5e-7a50-4fed-8eb7-c75622b4cd87', 37, NULL, NULL, '', NULL),
(110, 1060, 'a84ebf5e-7a50-4fed-8eb7-c75622b4cd87', 38, NULL, NULL, NULL, '<p><span>Formål:</span></p>\r\n<ol>\r\n<li>At få en bedre balance.</li>\r\n<li>At blive <strong>bedre</strong> til at klare svimmelhed.</li>\r\n</ol>'),
(111, 1060, 'a84ebf5e-7a50-4fed-8eb7-c75622b4cd87', 39, NULL, NULL, NULL, '<p><strong>asdfasdf</strong></p>'),
(112, 1060, 'a84ebf5e-7a50-4fed-8eb7-c75622b4cd87', 40, NULL, NULL, NULL, ''),
(113, 1060, 'a84ebf5e-7a50-4fed-8eb7-c75622b4cd87', 41, NULL, NULL, NULL, ''),
(114, 1060, '026adef7-e973-495c-990c-7f9a5b00f25a', 28, NULL, NULL, '6', NULL),
(115, 1060, '026adef7-e973-495c-990c-7f9a5b00f25a', 34, NULL, NULL, 'Vestibulærtræning', NULL),
(116, 1060, '026adef7-e973-495c-990c-7f9a5b00f25a', 35, NULL, NULL, NULL, NULL),
(117, 1060, '026adef7-e973-495c-990c-7f9a5b00f25a', 36, NULL, NULL, NULL, NULL),
(118, 1060, '026adef7-e973-495c-990c-7f9a5b00f25a', 37, NULL, NULL, NULL, NULL),
(119, 1060, '026adef7-e973-495c-990c-7f9a5b00f25a', 38, NULL, NULL, NULL, '<p><span>Formål:</span></p>\r\n<ol>\r\n<li>At få en bedre balance.</li>\r\n<li>At blive <strong>bedre</strong> til at klare svimmelhed.</li>\r\n</ol>'),
(120, 1060, '026adef7-e973-495c-990c-7f9a5b00f25a', 39, NULL, NULL, NULL, '<p><strong>asdfasdf</strong></p>'),
(121, 1060, '026adef7-e973-495c-990c-7f9a5b00f25a', 40, NULL, NULL, NULL, NULL),
(122, 1060, '026adef7-e973-495c-990c-7f9a5b00f25a', 41, NULL, NULL, NULL, NULL),
(123, 1071, 'd476e5c8-0d24-477a-bb75-5271c18da8f3', 50, NULL, NULL, '7', NULL),
(124, 1076, '91a03346-79e3-48a3-9a87-ef098b6bca96', 52, NULL, NULL, '1060', NULL),
(125, 1076, '91a03346-79e3-48a3-9a87-ef098b6bca96', 53, 1053, NULL, NULL, NULL),
(126, 1076, '91a03346-79e3-48a3-9a87-ef098b6bca96', 54, NULL, '2012-11-10 04:20:00', NULL, NULL),
(127, 1076, '91a03346-79e3-48a3-9a87-ef098b6bca96', 55, NULL, '2012-11-11 11:41:00', NULL, NULL),
(128, 1076, '91a03346-79e3-48a3-9a87-ef098b6bca96', 56, NULL, NULL, NULL, ''),
(129, 1076, '91a03346-79e3-48a3-9a87-ef098b6bca96', 57, 1, NULL, NULL, NULL),
(130, 1076, 'ab694aa2-bf3e-4c8b-b0b4-177dbfa04fd1', 52, NULL, NULL, '1060', NULL),
(131, 1076, 'ab694aa2-bf3e-4c8b-b0b4-177dbfa04fd1', 53, 1053, NULL, NULL, NULL),
(132, 1076, 'ab694aa2-bf3e-4c8b-b0b4-177dbfa04fd1', 54, NULL, '2012-11-10 04:20:00', NULL, NULL),
(133, 1076, 'ab694aa2-bf3e-4c8b-b0b4-177dbfa04fd1', 55, NULL, '2012-11-11 11:41:00', NULL, NULL),
(134, 1076, 'ab694aa2-bf3e-4c8b-b0b4-177dbfa04fd1', 56, NULL, NULL, NULL, NULL),
(135, 1076, 'ab694aa2-bf3e-4c8b-b0b4-177dbfa04fd1', 57, 1, NULL, NULL, NULL),
(136, 1062, 'ad9fbc13-e861-4441-b27f-e6e617fcfb16', 42, NULL, NULL, 'Intro', NULL),
(137, 1062, 'ad9fbc13-e861-4441-b27f-e6e617fcfb16', 43, NULL, NULL, 'Intro', NULL),
(138, 1062, 'ad9fbc13-e861-4441-b27f-e6e617fcfb16', 44, NULL, NULL, 'Intro', NULL),
(139, 1062, 'ad9fbc13-e861-4441-b27f-e6e617fcfb16', 45, NULL, NULL, 'Intro', NULL),
(140, 1062, 'ad9fbc13-e861-4441-b27f-e6e617fcfb16', 30, NULL, NULL, NULL, '<ol>\r\n<li>Denne øvelse er opdelt i 7 forskellige trin, der træner hver deres del af labyrintsansen. Læs evt. mere om labyrintsansen under balancen.</li>\r\n<li>Har man problemer med balancen skal man lave denne øvelse i små portioner, da de kan fremkalde svimmelhed og ubehag.</li>\r\n<li>Det er ikke sikkert, at man har problemer med alle delene. Hvis man kan lave to af trinene uden problemer, skal man bare gå videre til næste.</li>\r\n</ol>'),
(141, 1062, 'ad9fbc13-e861-4441-b27f-e6e617fcfb16', 31, NULL, NULL, NULL, ''),
(142, 1062, 'ad9fbc13-e861-4441-b27f-e6e617fcfb16', 32, NULL, NULL, NULL, ''),
(143, 1062, 'ad9fbc13-e861-4441-b27f-e6e617fcfb16', 33, NULL, NULL, NULL, ''),
(144, 1062, '7dbbb112-640f-48f0-8c8c-b31f8811983a', 42, NULL, NULL, 'Intro', NULL),
(145, 1062, '7dbbb112-640f-48f0-8c8c-b31f8811983a', 43, NULL, NULL, 'Intro', NULL),
(146, 1062, '7dbbb112-640f-48f0-8c8c-b31f8811983a', 44, NULL, NULL, 'Intro', NULL),
(147, 1062, '7dbbb112-640f-48f0-8c8c-b31f8811983a', 45, NULL, NULL, 'Intro', NULL),
(148, 1062, '7dbbb112-640f-48f0-8c8c-b31f8811983a', 30, NULL, NULL, NULL, '<ol>\r\n<li>Denne øvelse er opdelt i 7 forskellige trin, der træner hver deres del af labyrintsansen. Læs evt. mere om labyrintsansen under balancen.</li>\r\n<li>Har man problemer med balancen skal man lave denne øvelse i små portioner, da de kan fremkalde svimmelhed og ubehag.</li>\r\n<li>Det er ikke sikkert, at man har problemer med alle delene. Hvis man kan lave to af trinene uden problemer, skal man bare gå videre til næste.</li>\r\n</ol>'),
(149, 1062, '7dbbb112-640f-48f0-8c8c-b31f8811983a', 31, NULL, NULL, NULL, NULL),
(150, 1062, '7dbbb112-640f-48f0-8c8c-b31f8811983a', 32, NULL, NULL, NULL, NULL),
(151, 1062, '7dbbb112-640f-48f0-8c8c-b31f8811983a', 33, NULL, NULL, NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `CMSPROPERTYTYPE`
--

CREATE TABLE IF NOT EXISTS `CMSPROPERTYTYPE` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `DATATYPEID` int(11) NOT NULL,
  `CONTENTTYPEID` int(11) NOT NULL,
  `TABID` int(11) DEFAULT NULL,
  `ALIAS` varchar(255) CHARACTER SET utf8 NOT NULL,
  `NAME` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `HELPTEXT` varchar(1000) CHARACTER SET utf8 DEFAULT NULL,
  `SORTORDER` int(11) NOT NULL DEFAULT '0',
  `MANDATORY` bit(1) NOT NULL DEFAULT b'0',
  `VALIDATIONREGEXP` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `DESCRIPTION` varchar(2000) CHARACTER SET utf8 DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `TABID` (`TABID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=58 ;

--
-- Dumping data for table `CMSPROPERTYTYPE`
--

INSERT INTO `CMSPROPERTYTYPE` (`ID`, `DATATYPEID`, `CONTENTTYPEID`, `TABID`, `ALIAS`, `NAME`, `HELPTEXT`, `SORTORDER`, `MANDATORY`, `VALIDATIONREGEXP`, `DESCRIPTION`) VALUES
(6, -90, 1032, 3, 'umbracoFile', 'Upload image', NULL, 0, '\0', NULL, NULL),
(7, -92, 1032, 3, 'umbracoWidth', 'Width', NULL, 0, '\0', NULL, NULL),
(8, -92, 1032, 3, 'umbracoHeight', 'Height', NULL, 0, '\0', NULL, NULL),
(9, -92, 1032, 3, 'umbracoBytes', 'Size', NULL, 0, '\0', NULL, NULL),
(10, -92, 1032, 3, 'umbracoExtension', 'Type', NULL, 0, '\0', NULL, NULL),
(24, -90, 1033, 4, 'umbracoFile', 'Upload file', NULL, 0, '\0', NULL, NULL),
(25, -92, 1033, 4, 'umbracoExtension', 'Type', NULL, 0, '\0', NULL, NULL),
(26, -92, 1033, 4, 'umbracoBytes', 'Size', NULL, 0, '\0', NULL, NULL),
(27, -38, 1031, 5, 'contents', 'Contents:', NULL, 0, '\0', NULL, NULL),
(28, 1061, 1056, 6, 'type', 'Type', NULL, 0, '', '', ''),
(30, -87, 1057, 7, 'textDk', 'Text Dk', NULL, 4, '\0', '', ''),
(31, -87, 1057, 7, 'textDe', 'Text De', NULL, 5, '\0', '', ''),
(32, -87, 1057, 7, 'textGb', 'Text Gb', NULL, 6, '\0', '', ''),
(33, -87, 1057, 7, 'textNo', 'Text No', NULL, 7, '\0', '', ''),
(34, -88, 1056, 6, 'nameDk', 'Name Dk', NULL, 0, '\0', '', ''),
(35, -88, 1056, 6, 'nameDe', 'Name De', NULL, 0, '\0', '', ''),
(36, -88, 1056, 6, 'nameGb', 'Name Gb', NULL, 0, '\0', '', ''),
(37, -88, 1056, 6, 'nameNo', 'Name No', NULL, 0, '\0', '', ''),
(38, -87, 1056, 6, 'textDk', 'Text Dk', NULL, 0, '\0', '', ''),
(39, -87, 1056, 6, 'textDe', 'Text De', NULL, 0, '\0', '', ''),
(40, -87, 1056, 6, 'textGb', 'Text Gb', NULL, 0, '\0', '', ''),
(41, -87, 1056, 6, 'textNo', 'Text No', NULL, 0, '\0', '', ''),
(42, -88, 1057, 7, 'headingDk', 'Heading Dk', NULL, 0, '\0', '', ''),
(43, -88, 1057, 7, 'headingDe', 'Heading De', NULL, 1, '\0', '', ''),
(44, -88, 1057, 7, 'headingGb', 'Heading Gb', NULL, 2, '\0', '', ''),
(45, -88, 1057, 7, 'headingNo', 'Heading No', NULL, 3, '\0', '', ''),
(46, -88, 1063, 8, 'seoPageTitle', 'Page Title', NULL, 0, '\0', '', ''),
(47, -88, 1063, 8, 'seoKeywords', 'Keywords', NULL, 0, '\0', '', ''),
(48, -88, 1063, 8, 'seoDescription', 'Description', NULL, 0, '\0', '', ''),
(49, -87, 1063, 9, 'bodyText', 'Body text', NULL, 0, '\0', '', ''),
(50, 1070, 1046, NULL, 'country', 'Country', NULL, 0, '\0', '', ''),
(52, 1075, 1074, NULL, 'exercise', 'Exercise', NULL, 0, '\0', '', ''),
(53, 1036, 1074, NULL, 'member', 'Member', NULL, 0, '\0', '', ''),
(54, -36, 1074, NULL, 'timeStart', 'Time start', NULL, 0, '\0', '', ''),
(55, -36, 1074, NULL, 'timeEnd', 'Time end', NULL, 0, '\0', '', ''),
(56, -89, 1074, NULL, 'note', 'Note', NULL, 0, '\0', '', ''),
(57, -51, 1074, NULL, 'score', 'Score', NULL, 0, '\0', '', '');

-- --------------------------------------------------------

--
-- Table structure for table `CMSSTYLESHEET`
--

CREATE TABLE IF NOT EXISTS `CMSSTYLESHEET` (
  `NODEID` int(11) NOT NULL,
  `FILENAME` varchar(100) CHARACTER SET utf8 NOT NULL,
  `CONTENT` longtext COLLATE latin1_danish_ci
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci;

-- --------------------------------------------------------

--
-- Table structure for table `CMSSTYLESHEETPROPERTY`
--

CREATE TABLE IF NOT EXISTS `CMSSTYLESHEETPROPERTY` (
  `NODEID` int(11) NOT NULL,
  `STYLESHEETPROPERTYEDITOR` bit(1) DEFAULT NULL,
  `STYLESHEETPROPERTYALIAS` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `STYLESHEETPROPERTYVALUE` varchar(400) CHARACTER SET utf8 DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci;

-- --------------------------------------------------------

--
-- Table structure for table `CMSTAB`
--

CREATE TABLE IF NOT EXISTS `CMSTAB` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `CONTENTTYPENODEID` int(11) NOT NULL,
  `TEXT` varchar(255) CHARACTER SET utf8 NOT NULL,
  `SORTORDER` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=10 ;

--
-- Dumping data for table `CMSTAB`
--

INSERT INTO `CMSTAB` (`ID`, `CONTENTTYPENODEID`, `TEXT`, `SORTORDER`) VALUES
(3, 1032, 'Image', 1),
(4, 1033, 'File', 1),
(5, 1031, 'Contents', 1),
(6, 1056, 'Contents', 1),
(7, 1057, 'Contents', 1),
(8, 1063, 'SEO', 2),
(9, 1063, 'Contents', 1);

-- --------------------------------------------------------

--
-- Table structure for table `CMSTAGRELATIONSHIP`
--

CREATE TABLE IF NOT EXISTS `CMSTAGRELATIONSHIP` (
  `NODEID` int(11) NOT NULL,
  `TAGID` int(11) NOT NULL,
  PRIMARY KEY (`NODEID`,`TAGID`),
  KEY `CMSTAGS_CMSTAGRELATIONSHIP` (`TAGID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci;

-- --------------------------------------------------------

--
-- Table structure for table `CMSTAGS`
--

CREATE TABLE IF NOT EXISTS `CMSTAGS` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `TAG` varchar(200) COLLATE latin1_danish_ci DEFAULT NULL,
  `PARENTID` int(11) DEFAULT NULL,
  `GROUP` varchar(100) COLLATE latin1_danish_ci DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `CMSTASK`
--

CREATE TABLE IF NOT EXISTS `CMSTASK` (
  `CLOSED` bit(1) NOT NULL DEFAULT b'0',
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `TASKTYPEID` tinyint(4) NOT NULL,
  `NODEID` int(11) NOT NULL,
  `PARENTUSERID` int(11) NOT NULL,
  `USERID` int(11) NOT NULL,
  `DATETIME` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `COMMENT` varchar(500) CHARACTER SET utf8 DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `CMSTASKTYPE`
--

CREATE TABLE IF NOT EXISTS `CMSTASKTYPE` (
  `ID` tinyint(4) NOT NULL AUTO_INCREMENT,
  `ALIAS` varchar(255) CHARACTER SET utf8 NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=2 ;

--
-- Dumping data for table `CMSTASKTYPE`
--

INSERT INTO `CMSTASKTYPE` (`ID`, `ALIAS`) VALUES
(1, 'toTranslate');

-- --------------------------------------------------------

--
-- Table structure for table `CMSTEMPLATE`
--

CREATE TABLE IF NOT EXISTS `CMSTEMPLATE` (
  `PK` int(11) NOT NULL AUTO_INCREMENT,
  `NODEID` int(11) NOT NULL,
  `MASTER` int(11) DEFAULT NULL,
  `ALIAS` varchar(100) CHARACTER SET utf8 DEFAULT NULL,
  `DESIGN` longtext COLLATE latin1_danish_ci NOT NULL,
  PRIMARY KEY (`PK`),
  KEY `NODEID` (`NODEID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=3 ;

--
-- Dumping data for table `CMSTEMPLATE`
--

INSERT INTO `CMSTEMPLATE` (`PK`, `NODEID`, `MASTER`, `ALIAS`, `DESIGN`) VALUES
(1, 1064, NULL, 'master', '<%@ Master Language="C#" MasterPageFile="~/umbraco/masterpages/default.master" AutoEventWireup="true" %>\n<asp:Content ContentPlaceHolderID="ContentPlaceHolderDefault" runat="server">\n\n	<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">\n<html xmlns="http://www.w3.org/1999/xhtml">\n<head><title>\n	<umbraco:Item field="seoPageTitle" stripParagraph="true" runat="server" />\n</title><link rel="Stylesheet" href="/css/style.css" />\n	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>\n</head>\n<body>\n\n	\n<div style="position: relative; margin: auto; width: 1000px;">\n		<div id="Panel4" style="border-color:#D4D4D4;border-width:1px;border-style:solid;width:1000px;">\n	\n			<!-- border grå -->\n			<table cellpadding="0px" border="0px" cellspacing="0px" style="background-color: White">\n				<tr>\n					<td>\n					</td>\n					<td style="text-align:center;background-color:#ffffff;">\n					</td>\n				</tr>\n				<tr>\n					<td>\n						<div style="padding: 0px; background-color: #c1b5a5; width: 770px; height: 55px;\n							text-align: left">\n							<!-- 636466 -->\n							<div style="height: 15px">\n								&nbsp;</div>\n							<div style="margin-left: 40px" class="topmenu">\n								<a href="/">Home</a>\n								<!-- <navigation> -->\n<umbraco:Macro Alias="Navigation" runat="server"></umbraco:Macro>\n								<!-- </navigation> -->\n							</div>\n						</div>\n					</td>\n					<td>\n						<div style="margin: 0px; padding: 0px; background-color: #f78e1e; width: 230px; height: 55px;\n							color: #FFFFFF;">\n							<div style="font-family: Verdana; font-size: 11px; text-align: center">\n								&nbsp;</div>\n							<div style="margin-left: 20px; font-family: Verdana; font-size: 20px; text-align: left">\n								In focus:</div>\n						</div>\n					</td>\n				</tr>\n				<tr>\n					<td align="left" valign="top">\n						<div style="margin-left: 40px; margin-top: 60px; min-height: 750px; width: 650px; font-family: Verdana;\n							font-size: 11px; color: #636466" class="mainContents">\n								<asp:ContentPlaceHolder Id="CPHdefault" runat="server">\n									<!-- Insert default "CPHdefault" markup here -->\n								</asp:ContentPlaceHolder>\n						</div>\n					</td>\n					<td align="left" valign="top">\n						<div id="Panel2" style="background-image:url(img/streg.jpg);">\n		\n							&nbsp;\n							<div class="infocus" style="margin-left: 20px; margin-top: 47px; height: 750px; width: 200px; font-family: Verdana;\n								font-size: 11px; color: #636466">\n							</div>\n						\n	</div>\n					</td>\n				</tr>\n			</table>\n		\n</div>\n		\n	</div>\n	</body>\n	</html>\n\n\n	\n	\n</asp:Content>'),
(2, 1066, 1064, 'page', '<%@ Master Language="C#" MasterPageFile="~/masterpages/master.master" AutoEventWireup="true" %>\n	\n	\n<asp:Content ContentPlaceHolderId="CPHdefault" runat="server">\n	<!-- Insert "CPHdefault" markup here -->\n	<umbraco:Item field="bodyText" runat="server" />\n</asp:Content>');

-- --------------------------------------------------------

--
-- Table structure for table `UMBRACOAPP`
--

CREATE TABLE IF NOT EXISTS `UMBRACOAPP` (
  `SORTORDER` tinyint(4) NOT NULL DEFAULT '0',
  `APPALIAS` varchar(50) CHARACTER SET utf8 NOT NULL,
  `APPICON` varchar(255) CHARACTER SET utf8 NOT NULL,
  `APPNAME` varchar(255) CHARACTER SET utf8 NOT NULL,
  `APPINITWITHTREEALIAS` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  PRIMARY KEY (`APPALIAS`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci;

-- --------------------------------------------------------

--
-- Table structure for table `UMBRACOAPPTREE`
--

CREATE TABLE IF NOT EXISTS `UMBRACOAPPTREE` (
  `TREESILENT` bit(1) NOT NULL DEFAULT b'0',
  `TREEINITIALIZE` bit(1) NOT NULL DEFAULT b'1',
  `TREESORTORDER` tinyint(4) NOT NULL,
  `APPALIAS` varchar(50) CHARACTER SET utf8 NOT NULL,
  `TREEALIAS` varchar(150) CHARACTER SET utf8 NOT NULL,
  `TREETITLE` varchar(255) CHARACTER SET utf8 NOT NULL,
  `TREEICONCLOSED` varchar(255) CHARACTER SET utf8 NOT NULL,
  `TREEICONOPEN` varchar(255) CHARACTER SET utf8 NOT NULL,
  `TREEHANDLERASSEMBLY` varchar(255) CHARACTER SET utf8 NOT NULL,
  `TREEHANDLERTYPE` varchar(255) CHARACTER SET utf8 NOT NULL,
  `ACTION` varchar(300) CHARACTER SET utf8 DEFAULT NULL,
  PRIMARY KEY (`APPALIAS`,`TREEALIAS`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci;

-- --------------------------------------------------------

--
-- Table structure for table `UMBRACODOMAINS`
--

CREATE TABLE IF NOT EXISTS `UMBRACODOMAINS` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `DOMAINDEFAULTLANGUAGE` int(11) DEFAULT NULL,
  `DOMAINROOTSTRUCTUREID` int(11) DEFAULT NULL,
  `DOMAINNAME` varchar(255) CHARACTER SET utf8 NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `UMBRACOLANGUAGE`
--

CREATE TABLE IF NOT EXISTS `UMBRACOLANGUAGE` (
  `ID` smallint(6) NOT NULL AUTO_INCREMENT,
  `LANGUAGEISOCODE` varchar(10) CHARACTER SET utf8 DEFAULT NULL,
  `LANGUAGECULTURENAME` varchar(100) CHARACTER SET utf8 DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=5 ;

--
-- Dumping data for table `UMBRACOLANGUAGE`
--

INSERT INTO `UMBRACOLANGUAGE` (`ID`, `LANGUAGEISOCODE`, `LANGUAGECULTURENAME`) VALUES
(1, 'en-US', 'en-US'),
(2, 'da-DK', NULL),
(3, 'nb-NO', NULL),
(4, 'de-DE', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `UMBRACOLOG`
--

CREATE TABLE IF NOT EXISTS `UMBRACOLOG` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `USERID` int(11) NOT NULL,
  `NODEID` int(11) NOT NULL,
  `DATESTAMP` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LOGHEADER` varchar(50) CHARACTER SET utf8 NOT NULL,
  `LOGCOMMENT` varchar(4000) CHARACTER SET utf8 DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=274 ;

--
-- Dumping data for table `UMBRACOLOG`
--

INSERT INTO `UMBRACOLOG` (`ID`, `USERID`, `NODEID`, `DATESTAMP`, `LOGHEADER`, `LOGCOMMENT`) VALUES
(1, 0, -1, '2012-11-09 15:44:09', 'Debug', 'Republishing starting'),
(2, 0, -1, '2012-11-09 15:44:09', 'System', 'Loading content from database...'),
(3, 0, -1, '2012-11-09 15:44:09', 'Debug', 'Xml Pages loaded'),
(4, 0, -1, '2012-11-09 15:44:09', 'Debug', 'Xml saved in 00:00:00.0083107'),
(5, 0, -1, '2012-11-09 15:44:09', 'Debug', 'Done republishing Xml Index'),
(6, 0, -1, '2012-11-09 15:44:09', 'Debug', 'Xml saved in 00:00:00.0142057'),
(7, 0, -1, '2012-11-09 15:45:28', 'Login', ''),
(8, 0, -1, '2012-11-09 15:45:35', 'System', 'Application started at 09-11-2012 16:45:35'),
(9, 0, -1, '2012-11-09 15:45:37', 'System', 'Loading content from disk cache...'),
(10, 0, -1, '2012-11-09 15:45:37', 'Custom', '[UmbracoExamine] Adding examine event handlers for index providers: 3'),
(11, 0, -1, '2012-11-09 15:46:01', 'Debug', 'return:members/EditMemberType.aspx?id=1044'),
(12, 0, -1, '2012-11-09 15:46:06', 'Debug', 'return:members/EditMemberType.aspx?id=1045'),
(13, 0, -1, '2012-11-09 15:46:11', 'Debug', 'return:members/EditMemberType.aspx?id=1046'),
(14, 0, -1, '2012-11-09 15:46:17', 'Debug', 'return:members/EditMemberType.aspx?id=1047'),
(15, 0, -1, '2012-11-09 15:46:21', 'Debug', 'return:members/EditMemberType.aspx?id=1048'),
(16, 0, -1, '2012-11-09 15:46:27', 'Debug', 'return:members/EditMemberGroup.aspx?id=Administrators'),
(17, 0, -1, '2012-11-09 15:46:32', 'Debug', 'return:members/EditMemberGroup.aspx?id=Partners'),
(18, 0, -1, '2012-11-09 15:46:37', 'Debug', 'return:members/EditMemberGroup.aspx?id=Opticians'),
(19, 0, -1, '2012-11-09 15:46:43', 'Debug', 'return:members/EditMemberGroup.aspx?id=Clients'),
(20, 0, -1, '2012-11-09 15:48:45', 'Debug', 'return:developer/datatypes/editDataType.aspx?id=1054'),
(21, 0, -1, '2012-11-09 16:23:43', 'NotFound', ' (from '''')'),
(22, 0, -1, '2012-11-09 16:23:55', 'Login', ''),
(23, 0, -1, '2012-11-09 16:34:18', 'Debug', 'return:settings/editDictionaryItem.aspx?id=1'),
(24, 0, 1059, '2012-11-09 16:36:58', 'New', ''),
(25, 0, 1059, '2012-11-09 16:37:00', 'Open', ''),
(26, 0, 1060, '2012-11-09 19:28:03', 'New', ''),
(27, 0, 1060, '2012-11-09 19:28:03', 'Open', ''),
(28, 0, -1, '2012-11-09 19:31:08', 'Debug', 'return:developer/datatypes/editDataType.aspx?id=1061'),
(29, 0, 1060, '2012-11-09 19:32:31', 'Open', ''),
(30, 0, 1060, '2012-11-09 19:34:45', 'Open', ''),
(31, 0, 1060, '2012-11-09 19:40:00', 'Open', ''),
(32, 0, 1060, '2012-11-09 19:56:02', 'Open', ''),
(33, 0, 1062, '2012-11-09 19:56:34', 'New', ''),
(34, 0, 1062, '2012-11-09 19:56:35', 'Open', ''),
(35, 0, 1062, '2012-11-09 19:57:03', 'Open', ''),
(36, 0, 1062, '2012-11-09 19:57:12', 'Open', ''),
(37, 0, 1060, '2012-11-09 19:57:18', 'Open', ''),
(38, 0, 1060, '2012-11-09 19:58:10', 'Open', ''),
(39, 0, 1062, '2012-11-09 19:58:12', 'Open', ''),
(40, 0, 1062, '2012-11-09 19:59:48', 'Open', ''),
(41, 0, 1060, '2012-11-09 20:00:26', 'Open', ''),
(42, 0, 1059, '2012-11-09 20:00:29', 'Open', ''),
(43, 0, 1059, '2012-11-09 20:00:37', 'Publish', ''),
(44, 0, 1060, '2012-11-09 20:00:37', 'Publish', ''),
(45, 0, 1062, '2012-11-09 20:00:37', 'Publish', ''),
(46, 0, -1, '2012-11-09 20:00:37', 'System', 'Loading content from database...'),
(47, 0, -1, '2012-11-09 20:00:37', 'Debug', 'Republishing starting'),
(48, 0, -1, '2012-11-09 20:00:37', 'Debug', 'Xml Pages loaded'),
(49, 0, -1, '2012-11-09 20:00:37', 'Debug', 'Done republishing Xml Index'),
(50, 0, 1059, '2012-11-09 20:00:37', 'Error', 'Error adding to SiteMapProvider in loadNodes(): System.InvalidOperationException: Multiple nodes with the same URL ''/'' were found. XmlSiteMapProvider requires that sitemap nodes have unique URLs.\r\n   at System.Web.StaticSiteMapProvider.AddNode(SiteMapNode node, SiteMapNode parentNode)\r\n   at umbraco.presentation.nodeFactory.UmbracoSiteMapProvider.loadNodes(String parentId, SiteMapNode parentNode)'),
(51, 0, -1, '2012-11-09 20:00:37', 'Debug', 'Xml saved in 00:00:00.0079942'),
(52, 0, 1062, '2012-11-09 20:00:42', 'Open', ''),
(53, 0, 1060, '2012-11-09 20:01:05', 'Open', ''),
(54, 0, 1060, '2012-11-09 20:01:38', 'Open', ''),
(55, 0, 1060, '2012-11-09 20:01:40', 'Save', ''),
(56, 0, 1060, '2012-11-09 20:01:55', 'Save', ''),
(57, 0, 1060, '2012-11-09 20:01:56', 'Publish', ''),
(58, 0, -1, '2012-11-09 20:01:56', 'Debug', 'Xml saved in 00:00:00.0117372'),
(59, 0, 1060, '2012-11-09 20:02:35', 'Save', ''),
(60, 0, 1060, '2012-11-09 20:02:36', 'Publish', ''),
(61, 0, -1, '2012-11-09 20:02:36', 'Debug', 'Xml saved in 00:00:00.0110448'),
(62, 0, 1059, '2012-11-09 20:02:50', 'Open', ''),
(63, 0, 1059, '2012-11-09 20:05:14', 'Open', ''),
(64, 0, 1059, '2012-11-09 20:13:31', 'Open', ''),
(65, 0, 1067, '2012-11-09 20:13:41', 'New', ''),
(66, 0, 1067, '2012-11-09 20:13:42', 'Open', ''),
(67, 0, 1067, '2012-11-09 20:13:48', 'Open', ''),
(68, 0, 1067, '2012-11-09 20:13:54', 'Save', ''),
(69, 0, 1067, '2012-11-09 21:56:47', 'Open', ''),
(70, 0, 1067, '2012-11-09 21:56:54', 'Save', ''),
(71, 0, 1067, '2012-11-09 21:56:54', 'Publish', ''),
(72, 0, -1, '2012-11-09 21:56:54', 'System', 'Loading content from database...'),
(73, 0, -1, '2012-11-09 21:56:54', 'Debug', 'Republishing starting'),
(74, 0, -1, '2012-11-09 21:56:54', 'Debug', 'Xml Pages loaded'),
(75, 0, -1, '2012-11-09 21:56:54', 'Debug', 'Done republishing Xml Index'),
(76, 0, -1, '2012-11-09 21:56:54', 'Debug', 'Xml saved in 00:00:00.0203029'),
(77, 0, -1, '2012-11-09 21:57:10', 'Debug', 'Xml saved in 00:00:00.0101612'),
(78, 0, 1067, '2012-11-09 21:57:14', 'Open', ''),
(79, 0, 1059, '2012-11-09 21:57:22', 'Open', ''),
(80, 0, 1068, '2012-11-09 21:59:35', 'New', ''),
(81, 0, 1068, '2012-11-09 21:59:35', 'Open', ''),
(82, 0, 1067, '2012-11-09 21:59:37', 'Open', ''),
(83, 0, 1067, '2012-11-09 21:59:41', 'Open', ''),
(84, 0, 1068, '2012-11-09 21:59:45', 'Open', ''),
(85, 0, 1068, '2012-11-09 22:02:01', 'Open', ''),
(86, 0, 1068, '2012-11-09 22:02:05', 'Save', ''),
(87, 0, 1068, '2012-11-09 22:02:06', 'Publish', ''),
(88, 0, -1, '2012-11-09 22:02:06', 'Debug', 'Xml saved in 00:00:00.0110202'),
(89, 0, 1067, '2012-11-09 22:02:08', 'Open', ''),
(90, 0, 1067, '2012-11-09 22:02:18', 'Open', ''),
(91, 0, 1067, '2012-11-09 22:02:21', 'Open', ''),
(92, 0, 1068, '2012-11-09 22:02:22', 'Open', ''),
(93, 0, 1067, '2012-11-09 22:02:42', 'Open', ''),
(94, 0, 1067, '2012-11-09 22:02:49', 'Save', ''),
(95, 0, 1067, '2012-11-09 22:02:51', 'Save', ''),
(96, 0, 1067, '2012-11-09 22:02:52', 'Publish', ''),
(97, 0, -1, '2012-11-09 22:02:52', 'Debug', 'Xml saved in 00:00:00.0243173'),
(98, 0, 1068, '2012-11-09 22:02:56', 'Open', ''),
(99, 0, 1067, '2012-11-09 22:02:58', 'Open', ''),
(100, 0, 1067, '2012-11-09 22:03:20', 'Open', ''),
(101, 0, 1067, '2012-11-09 22:05:34', 'Open', ''),
(102, 0, -1, '2012-11-09 22:05:49', 'Error', 'At /1067.aspx (Referred by: http://umbraco.monosolutions.dk/umbraco/): System.ApplicationException: Error adding Canvas support. ---> System.ApplicationException: Umbraco Canvas requires an ASP.Net form to function properly. Live editing has been turned off.\r\n   at umbraco.presentation.masterpages._default.AddLiveEditingSupport()\r\n   at umbraco.presentation.masterpages._default.Page_Load(Object sender, EventArgs e)\r\n   --- End of inner exception stack trace ---\r\n   at umbraco.presentation.masterpages._default.Page_Load(Object sender, EventArgs e)\r\n   at System.Web.Util.CalliEventHandlerDelegateProxy.Callback(Object sender, EventArgs e)\r\n   at System.Web.UI.Control.OnLoad(EventArgs e)\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Page.ProcessRequestMain(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)'),
(103, 0, 1067, '2012-11-09 22:07:41', 'Open', ''),
(104, 0, 1069, '2012-11-09 22:08:03', 'New', ''),
(105, 0, 1069, '2012-11-09 22:08:04', 'Open', ''),
(106, 0, 1069, '2012-11-09 22:08:11', 'Save', ''),
(107, 0, 1069, '2012-11-09 22:08:13', 'Publish', ''),
(108, 0, -1, '2012-11-09 22:08:13', 'Debug', 'Xml saved in 00:00:00.0083288'),
(109, 0, 1069, '2012-11-09 22:09:13', 'Save', ''),
(110, 0, 1069, '2012-11-09 22:09:14', 'Publish', ''),
(111, 0, -1, '2012-11-09 22:09:14', 'Debug', 'Xml saved in 00:00:00.0117907'),
(112, 0, 1069, '2012-11-09 22:09:34', 'Save', ''),
(113, 0, 1069, '2012-11-09 22:09:34', 'Publish', ''),
(114, 0, -1, '2012-11-09 22:09:34', 'Debug', 'Xml saved in 00:00:00.0051881'),
(115, 0, 1067, '2012-11-09 22:09:36', 'Open', ''),
(116, 0, 1067, '2012-11-09 22:09:39', 'Save', ''),
(117, 0, 1067, '2012-11-09 22:09:39', 'Publish', ''),
(118, 0, -1, '2012-11-09 22:09:39', 'Debug', 'Xml saved in 00:00:00.0138570'),
(119, 0, 1067, '2012-11-09 22:10:19', 'Open', ''),
(120, 0, 1069, '2012-11-09 22:10:23', 'Open', ''),
(121, 0, 1069, '2012-11-09 22:10:27', 'Save', ''),
(122, 0, 1069, '2012-11-09 22:10:27', 'Publish', ''),
(123, 0, -1, '2012-11-09 22:10:27', 'Debug', 'Xml saved in 00:00:00.0082360'),
(124, 0, 1068, '2012-11-09 22:10:29', 'Open', ''),
(125, 0, 1068, '2012-11-09 22:10:35', 'Save', ''),
(126, 0, 1068, '2012-11-09 22:10:36', 'Publish', ''),
(127, 0, -1, '2012-11-09 22:10:36', 'Debug', 'Xml saved in 00:00:00.0107328'),
(128, 0, 1069, '2012-11-09 22:14:15', 'Open', ''),
(129, 0, 1068, '2012-11-09 22:14:49', 'Open', ''),
(130, 0, 1069, '2012-11-09 22:14:51', 'Open', ''),
(131, 0, 1060, '2012-11-09 22:15:17', 'Open', ''),
(132, 0, 1060, '2012-11-09 22:15:47', 'Save', ''),
(133, 0, 1060, '2012-11-09 22:15:47', 'Publish', ''),
(134, 0, -1, '2012-11-09 22:15:47', 'Debug', 'Xml saved in 00:00:00.0111332'),
(135, 0, 1060, '2012-11-10 07:11:18', 'Save', ''),
(136, 0, 1060, '2012-11-10 07:11:18', 'Publish', ''),
(137, 0, -1, '2012-11-10 07:11:18', 'Debug', 'Xml saved in 00:00:00.0142416'),
(138, 0, -1, '2012-11-10 07:24:28', 'Debug', 'return:developer/datatypes/editDataType.aspx?id=1070'),
(139, 0, -1, '2012-11-10 07:38:40', 'System', 'Application started at 10-11-2012 08:38:40'),
(140, 0, -1, '2012-11-10 07:38:42', 'System', 'Loading content from disk cache...'),
(141, 0, -1, '2012-11-10 07:38:42', 'Custom', '[UmbracoExamine] Adding examine event handlers for index providers: 3'),
(142, 0, -1, '2012-11-10 07:38:44', 'System', 'Loading content from database...'),
(143, 0, -1, '2012-11-10 07:38:44', 'Debug', 'Republishing starting'),
(144, 0, -1, '2012-11-10 07:38:44', 'Debug', 'Xml Pages loaded'),
(145, 0, -1, '2012-11-10 07:38:44', 'Debug', 'Done republishing Xml Index'),
(146, 0, -1, '2012-11-10 07:38:44', 'Debug', 'Xml saved in 00:00:00.0068738'),
(147, 0, -1, '2012-11-10 07:38:45', 'Debug', 'Xml saved in 00:00:00.0057713'),
(148, 0, -1, '2012-11-10 07:39:40', 'Debug', 'return:developer/datatypes/editDataType.aspx?id=1072'),
(149, 0, -1, '2012-11-10 07:45:22', 'Debug', 'return:developer/datatypes/editDataType.aspx?id=1073'),
(150, 0, -1, '2012-11-10 07:46:40', 'Error', 'At /umbraco/settings/editNodeTypeNew.aspx?id=1055 (Referred by: http://umbraco.monosolutions.dk/umbraco/settings/editNodeTypeNew.aspx?id=1055): System.NullReferenceException: Object reference not set to an instance of an object.\r\n   at Dascoba.Umb.CascadingProperties.Helper.<>c__DisplayClass26.<GetSerializedCascadingDataTypeDefinitionIds>b__22(ICascadingDataType cdt)\r\n   at System.Linq.Enumerable.Any[TSource](IEnumerable`1 source, Func`2 predicate)\r\n   at Dascoba.Umb.CascadingProperties.Helper.<GetSerializedCascadingDataTypeDefinitionIds>b__21(DataTypeDefinition dtd)\r\n   at System.Linq.Enumerable.WhereSelectArrayIterator`2.MoveNext()\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeEnumerable(IEnumerable enumerable, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValueInternal(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValue(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, StringBuilder output, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj)\r\n   at Dascoba.Umb.CascadingProperties.Helper.GetSerializedCascadingDataTypeDefinitionIds()\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.HandleEditPage(Page editPage)\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.umbracoPage_Load(Object sender, EventArgs e)\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.CMSNode_AfterSave(Object sender, SaveEventArgs e)\r\n   at umbraco.cms.businesslogic.CMSNode.FireAfterSave(SaveEventArgs e)\r\n   at umbraco.cms.businesslogic.CMSNode.Save()\r\n   at umbraco.cms.businesslogic.web.DocumentType.Save()\r\n   at umbraco.settings.EditContentTypeNew.OnBubbleEvent(Object source, EventArgs args)\r\n   at System.Web.UI.Control.RaiseBubbleEvent(Object source, EventArgs args)\r\n   at umbraco.controls.ContentTypeControlNew.save_click(Object sender, ImageClickEventArgs e)\r\n   at System.Web.UI.WebControls.ImageButton.OnClick(ImageClickEventArgs e)\r\n   at System.Web.UI.WebControls.ImageButton.RaisePostBackEvent(String eventArgument)\r\n   at System.Web.UI.WebControls.ImageButton.System.Web.UI.IPostBackEventHandler.RaisePostBackEvent(String eventArgument)\r\n   at System.Web.UI.Page.RaisePostBackEvent(IPostBackEventHandler sourceControl, String eventArgument)\r\n   at System.Web.UI.Page.RaisePostBackEvent(NameValueCollection postData)\r\n   at System.Web.UI.Page.ProcessRequestMain(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)'),
(151, 0, -1, '2012-11-10 07:46:42', 'Error', 'At /umbraco/settings/editNodeTypeNew.aspx?id=1055 (Referred by: http://umbraco.monosolutions.dk/umbraco/): System.NullReferenceException: Object reference not set to an instance of an object.\r\n   at Dascoba.Umb.CascadingProperties.Helper.<>c__DisplayClass26.<GetSerializedCascadingDataTypeDefinitionIds>b__22(ICascadingDataType cdt)\r\n   at System.Linq.Enumerable.Any[TSource](IEnumerable`1 source, Func`2 predicate)\r\n   at Dascoba.Umb.CascadingProperties.Helper.<GetSerializedCascadingDataTypeDefinitionIds>b__21(DataTypeDefinition dtd)\r\n   at System.Linq.Enumerable.WhereSelectArrayIterator`2.MoveNext()\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeEnumerable(IEnumerable enumerable, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValueInternal(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValue(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, StringBuilder output, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj)\r\n   at Dascoba.Umb.CascadingProperties.Helper.GetSerializedCascadingDataTypeDefinitionIds()\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.HandleEditPage(Page editPage)\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.umbracoPage_Load(Object sender, EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.FireOnLoad(EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.Page_Load(Object sender, EventArgs e)\r\n   at System.Web.Util.CalliEventHandlerDelegateProxy.Callback(Object sender, EventArgs e)\r\n   at System.Web.UI.Control.OnLoad(EventArgs e)\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Page.ProcessRequestMain(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)'),
(152, 0, -1, '2012-11-10 07:46:44', 'Error', 'At /umbraco/settings/editNodeTypeNew.aspx?id=1056 (Referred by: http://umbraco.monosolutions.dk/umbraco/): System.NullReferenceException: Object reference not set to an instance of an object.\r\n   at Dascoba.Umb.CascadingProperties.Helper.<>c__DisplayClass26.<GetSerializedCascadingDataTypeDefinitionIds>b__22(ICascadingDataType cdt)\r\n   at System.Linq.Enumerable.Any[TSource](IEnumerable`1 source, Func`2 predicate)\r\n   at Dascoba.Umb.CascadingProperties.Helper.<GetSerializedCascadingDataTypeDefinitionIds>b__21(DataTypeDefinition dtd)\r\n   at System.Linq.Enumerable.WhereSelectArrayIterator`2.MoveNext()\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeEnumerable(IEnumerable enumerable, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValueInternal(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValue(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, StringBuilder output, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj)\r\n   at Dascoba.Umb.CascadingProperties.Helper.GetSerializedCascadingDataTypeDefinitionIds()\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.HandleEditPage(Page editPage)\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.umbracoPage_Load(Object sender, EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.FireOnLoad(EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.Page_Load(Object sender, EventArgs e)\r\n   at System.Web.Util.CalliEventHandlerDelegateProxy.Callback(Object sender, EventArgs e)\r\n   at System.Web.UI.Control.OnLoad(EventArgs e)\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Page.ProcessRequestMain(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)'),
(153, 0, -1, '2012-11-10 07:46:54', 'Error', 'At /umbraco/settings/editNodeTypeNew.aspx?id=1055 (Referred by: http://umbraco.monosolutions.dk/umbraco/): System.NullReferenceException: Object reference not set to an instance of an object.\r\n   at Dascoba.Umb.CascadingProperties.Helper.<>c__DisplayClass26.<GetSerializedCascadingDataTypeDefinitionIds>b__22(ICascadingDataType cdt)\r\n   at System.Linq.Enumerable.Any[TSource](IEnumerable`1 source, Func`2 predicate)\r\n   at Dascoba.Umb.CascadingProperties.Helper.<GetSerializedCascadingDataTypeDefinitionIds>b__21(DataTypeDefinition dtd)\r\n   at System.Linq.Enumerable.WhereSelectArrayIterator`2.MoveNext()\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeEnumerable(IEnumerable enumerable, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValueInternal(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValue(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, StringBuilder output, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj)\r\n   at Dascoba.Umb.CascadingProperties.Helper.GetSerializedCascadingDataTypeDefinitionIds()\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.HandleEditPage(Page editPage)\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.umbracoPage_Load(Object sender, EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.FireOnLoad(EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.Page_Load(Object sender, EventArgs e)\r\n   at System.Web.Util.CalliEventHandlerDelegateProxy.Callback(Object sender, EventArgs e)\r\n   at System.Web.UI.Control.OnLoad(EventArgs e)\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Page.ProcessRequestMain(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)'),
(154, 0, -1, '2012-11-10 07:48:18', 'System', 'Application started at 10-11-2012 08:48:17'),
(155, 0, -1, '2012-11-10 07:48:19', 'System', 'Loading content from disk cache...'),
(156, 0, -1, '2012-11-10 07:48:19', 'Custom', '[UmbracoExamine] Adding examine event handlers for index providers: 3'),
(157, 0, -1, '2012-11-10 07:48:34', 'Error', 'At /umbraco/settings/editNodeTypeNew.aspx?id=1055 (Referred by: http://umbraco.monosolutions.dk/umbraco/): System.NullReferenceException: Object reference not set to an instance of an object.\r\n   at Dascoba.Umb.CascadingProperties.Helper.<>c__DisplayClass26.<GetSerializedCascadingDataTypeDefinitionIds>b__22(ICascadingDataType cdt)\r\n   at System.Linq.Enumerable.Any[TSource](IEnumerable`1 source, Func`2 predicate)\r\n   at Dascoba.Umb.CascadingProperties.Helper.<GetSerializedCascadingDataTypeDefinitionIds>b__21(DataTypeDefinition dtd)\r\n   at System.Linq.Enumerable.WhereSelectArrayIterator`2.MoveNext()\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeEnumerable(IEnumerable enumerable, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValueInternal(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValue(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, StringBuilder output, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj)\r\n   at Dascoba.Umb.CascadingProperties.Helper.GetSerializedCascadingDataTypeDefinitionIds()\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.HandleEditPage(Page editPage)\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.umbracoPage_Load(Object sender, EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.FireOnLoad(EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.Page_Load(Object sender, EventArgs e)\r\n   at System.Web.Util.CalliEventHandlerDelegateProxy.Callback(Object sender, EventArgs e)\r\n   at System.Web.UI.Control.OnLoad(EventArgs e)\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Page.ProcessRequestMain(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)'),
(158, 0, -1, '2012-11-10 07:49:09', 'System', 'Application started at 10-11-2012 08:49:09'),
(159, 0, -1, '2012-11-10 07:49:10', 'System', 'Loading content from disk cache...'),
(160, 0, -1, '2012-11-10 07:49:10', 'Custom', '[UmbracoExamine] Adding examine event handlers for index providers: 3'),
(161, 0, -1, '2012-11-10 07:49:23', 'Error', 'At /umbraco/settings/editNodeTypeNew.aspx?id=1055 (Referred by: http://umbraco.monosolutions.dk/umbraco/): System.NullReferenceException: Object reference not set to an instance of an object.\r\n   at Dascoba.Umb.CascadingProperties.Helper.<>c__DisplayClass26.<GetSerializedCascadingDataTypeDefinitionIds>b__22(ICascadingDataType cdt)\r\n   at System.Linq.Enumerable.Any[TSource](IEnumerable`1 source, Func`2 predicate)\r\n   at Dascoba.Umb.CascadingProperties.Helper.<GetSerializedCascadingDataTypeDefinitionIds>b__21(DataTypeDefinition dtd)\r\n   at System.Linq.Enumerable.WhereSelectArrayIterator`2.MoveNext()\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeEnumerable(IEnumerable enumerable, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValueInternal(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValue(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, StringBuilder output, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj)\r\n   at Dascoba.Umb.CascadingProperties.Helper.GetSerializedCascadingDataTypeDefinitionIds()\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.HandleEditPage(Page editPage)\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.umbracoPage_Load(Object sender, EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.FireOnLoad(EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.Page_Load(Object sender, EventArgs e)\r\n   at System.Web.Util.CalliEventHandlerDelegateProxy.Callback(Object sender, EventArgs e)\r\n   at System.Web.UI.Control.OnLoad(EventArgs e)\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Page.ProcessRequestMain(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)'),
(162, 0, -1, '2012-11-10 07:49:49', 'System', 'Loading content from database...'),
(163, 0, -1, '2012-11-10 07:49:49', 'Debug', 'Republishing starting'),
(164, 0, -1, '2012-11-10 07:49:49', 'Debug', 'Xml Pages loaded'),
(165, 0, -1, '2012-11-10 07:49:49', 'Debug', 'Done republishing Xml Index'),
(166, 0, -1, '2012-11-10 07:49:49', 'Debug', 'Xml saved in 00:00:00.0073709'),
(167, 0, -1, '2012-11-10 07:49:50', 'Debug', 'Xml saved in 00:00:00.0164499'),
(168, 0, -1, '2012-11-10 07:49:51', 'Error', 'At /umbraco/settings/editNodeTypeNew.aspx?id=1056 (Referred by: http://umbraco.monosolutions.dk/umbraco/): System.NullReferenceException: Object reference not set to an instance of an object.\r\n   at Dascoba.Umb.CascadingProperties.Helper.<>c__DisplayClass26.<GetSerializedCascadingDataTypeDefinitionIds>b__22(ICascadingDataType cdt)\r\n   at System.Linq.Enumerable.Any[TSource](IEnumerable`1 source, Func`2 predicate)\r\n   at Dascoba.Umb.CascadingProperties.Helper.<GetSerializedCascadingDataTypeDefinitionIds>b__21(DataTypeDefinition dtd)\r\n   at System.Linq.Enumerable.WhereSelectArrayIterator`2.MoveNext()\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeEnumerable(IEnumerable enumerable, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValueInternal(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValue(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, StringBuilder output, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj)\r\n   at Dascoba.Umb.CascadingProperties.Helper.GetSerializedCascadingDataTypeDefinitionIds()\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.HandleEditPage(Page editPage)\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.umbracoPage_Load(Object sender, EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.FireOnLoad(EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.Page_Load(Object sender, EventArgs e)\r\n   at System.Web.Util.CalliEventHandlerDelegateProxy.Callback(Object sender, EventArgs e)\r\n   at System.Web.UI.Control.OnLoad(EventArgs e)\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Page.ProcessRequestMain(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)'),
(169, 0, -1, '2012-11-10 07:49:56', 'Error', 'At /umbraco/settings/editNodeTypeNew.aspx?id=1056 (Referred by: http://umbraco.monosolutions.dk/umbraco/): System.NullReferenceException: Object reference not set to an instance of an object.\r\n   at Dascoba.Umb.CascadingProperties.Helper.<>c__DisplayClass26.<GetSerializedCascadingDataTypeDefinitionIds>b__22(ICascadingDataType cdt)\r\n   at System.Linq.Enumerable.Any[TSource](IEnumerable`1 source, Func`2 predicate)\r\n   at Dascoba.Umb.CascadingProperties.Helper.<GetSerializedCascadingDataTypeDefinitionIds>b__21(DataTypeDefinition dtd)\r\n   at System.Linq.Enumerable.WhereSelectArrayIterator`2.MoveNext()\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeEnumerable(IEnumerable enumerable, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValueInternal(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValue(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, StringBuilder output, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj)\r\n   at Dascoba.Umb.CascadingProperties.Helper.GetSerializedCascadingDataTypeDefinitionIds()\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.HandleEditPage(Page editPage)\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.umbracoPage_Load(Object sender, EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.FireOnLoad(EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.Page_Load(Object sender, EventArgs e)\r\n   at System.Web.Util.CalliEventHandlerDelegateProxy.Callback(Object sender, EventArgs e)\r\n   at System.Web.UI.Control.OnLoad(EventArgs e)\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Page.ProcessRequestMain(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)'),
(170, 0, -1, '2012-11-10 07:50:37', 'Error', 'At /umbraco/settings/editNodeTypeNew.aspx?id=1056 (Referred by: http://umbraco.monosolutions.dk/umbraco/): System.NullReferenceException: Object reference not set to an instance of an object.\r\n   at Dascoba.Umb.CascadingProperties.Helper.<>c__DisplayClass26.<GetSerializedCascadingDataTypeDefinitionIds>b__22(ICascadingDataType cdt)\r\n   at System.Linq.Enumerable.Any[TSource](IEnumerable`1 source, Func`2 predicate)\r\n   at Dascoba.Umb.CascadingProperties.Helper.<GetSerializedCascadingDataTypeDefinitionIds>b__21(DataTypeDefinition dtd)\r\n   at System.Linq.Enumerable.WhereSelectArrayIterator`2.MoveNext()\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeEnumerable(IEnumerable enumerable, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValueInternal(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValue(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, StringBuilder output, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj)\r\n   at Dascoba.Umb.CascadingProperties.Helper.GetSerializedCascadingDataTypeDefinitionIds()\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.HandleEditPage(Page editPage)\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.umbracoPage_Load(Object sender, EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.FireOnLoad(EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.Page_Load(Object sender, EventArgs e)\r\n   at System.Web.Util.CalliEventHandlerDelegateProxy.Callback(Object sender, EventArgs e)\r\n   at System.Web.UI.Control.OnLoad(EventArgs e)\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Page.ProcessRequestMain(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)'),
(171, 0, -1, '2012-11-10 07:50:38', 'Error', 'At /umbraco/settings/editNodeTypeNew.aspx?id=1063 (Referred by: http://umbraco.monosolutions.dk/umbraco/): System.NullReferenceException: Object reference not set to an instance of an object.\r\n   at Dascoba.Umb.CascadingProperties.Helper.<>c__DisplayClass26.<GetSerializedCascadingDataTypeDefinitionIds>b__22(ICascadingDataType cdt)\r\n   at System.Linq.Enumerable.Any[TSource](IEnumerable`1 source, Func`2 predicate)\r\n   at Dascoba.Umb.CascadingProperties.Helper.<GetSerializedCascadingDataTypeDefinitionIds>b__21(DataTypeDefinition dtd)\r\n   at System.Linq.Enumerable.WhereSelectArrayIterator`2.MoveNext()\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeEnumerable(IEnumerable enumerable, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValueInternal(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValue(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, StringBuilder output, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj)\r\n   at Dascoba.Umb.CascadingProperties.Helper.GetSerializedCascadingDataTypeDefinitionIds()\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.HandleEditPage(Page editPage)\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.umbracoPage_Load(Object sender, EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.FireOnLoad(EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.Page_Load(Object sender, EventArgs e)\r\n   at System.Web.Util.CalliEventHandlerDelegateProxy.Callback(Object sender, EventArgs e)\r\n   at System.Web.UI.Control.OnLoad(EventArgs e)\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Page.ProcessRequestMain(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)'),
(172, 0, -1, '2012-11-10 07:50:45', 'Error', 'At /umbraco/settings/editNodeTypeNew.aspx?id=1057 (Referred by: http://umbraco.monosolutions.dk/umbraco/): System.NullReferenceException: Object reference not set to an instance of an object.\r\n   at Dascoba.Umb.CascadingProperties.Helper.<>c__DisplayClass26.<GetSerializedCascadingDataTypeDefinitionIds>b__22(ICascadingDataType cdt)\r\n   at System.Linq.Enumerable.Any[TSource](IEnumerable`1 source, Func`2 predicate)\r\n   at Dascoba.Umb.CascadingProperties.Helper.<GetSerializedCascadingDataTypeDefinitionIds>b__21(DataTypeDefinition dtd)\r\n   at System.Linq.Enumerable.WhereSelectArrayIterator`2.MoveNext()\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeEnumerable(IEnumerable enumerable, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValueInternal(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValue(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, StringBuilder output, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj)\r\n   at Dascoba.Umb.CascadingProperties.Helper.GetSerializedCascadingDataTypeDefinitionIds()\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.HandleEditPage(Page editPage)\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.umbracoPage_Load(Object sender, EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.FireOnLoad(EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.Page_Load(Object sender, EventArgs e)\r\n   at System.Web.Util.CalliEventHandlerDelegateProxy.Callback(Object sender, EventArgs e)\r\n   at System.Web.UI.Control.OnLoad(EventArgs e)\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Page.ProcessRequestMain(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)'),
(173, 0, -1, '2012-11-10 07:50:49', 'Error', 'At /umbraco/settings/editNodeTypeNew.aspx?id=1056 (Referred by: http://umbraco.monosolutions.dk/umbraco/): System.NullReferenceException: Object reference not set to an instance of an object.\r\n   at Dascoba.Umb.CascadingProperties.Helper.<>c__DisplayClass26.<GetSerializedCascadingDataTypeDefinitionIds>b__22(ICascadingDataType cdt)\r\n   at System.Linq.Enumerable.Any[TSource](IEnumerable`1 source, Func`2 predicate)\r\n   at Dascoba.Umb.CascadingProperties.Helper.<GetSerializedCascadingDataTypeDefinitionIds>b__21(DataTypeDefinition dtd)\r\n   at System.Linq.Enumerable.WhereSelectArrayIterator`2.MoveNext()\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeEnumerable(IEnumerable enumerable, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValueInternal(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.SerializeValue(Object o, StringBuilder sb, Int32 depth, Hashtable objectsInUse, SerializationFormat serializationFormat, MemberInfo currentMember)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, StringBuilder output, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, SerializationFormat serializationFormat)\r\n   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj)\r\n   at Dascoba.Umb.CascadingProperties.Helper.GetSerializedCascadingDataTypeDefinitionIds()\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.HandleEditPage(Page editPage)\r\n   at Dascoba.Umb.CascadingProperties.ApplicationEvents.umbracoPage_Load(Object sender, EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.FireOnLoad(EventArgs e)\r\n   at umbraco.presentation.masterpages.umbracoPage.Page_Load(Object sender, EventArgs e)\r\n   at System.Web.Util.CalliEventHandlerDelegateProxy.Callback(Object sender, EventArgs e)\r\n   at System.Web.UI.Control.OnLoad(EventArgs e)\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Control.LoadRecursive()\r\n   at System.Web.UI.Page.ProcessRequestMain(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)'),
(174, 0, -1, '2012-11-10 07:51:23', 'Debug', 'executing undo actions'),
(175, 0, -1, '2012-11-10 07:51:23', 'Debug', '<Actions></Actions>'),
(176, 0, -1, '2012-11-10 07:51:27', 'System', 'Application started at 10-11-2012 08:51:26'),
(177, 0, -1, '2012-11-10 07:51:28', 'System', 'Loading content from disk cache...'),
(178, 0, -1, '2012-11-10 07:51:28', 'Custom', '[UmbracoExamine] Adding examine event handlers for index providers: 3'),
(179, 0, 1062, '2012-11-10 14:21:50', 'Open', ''),
(180, 0, -1, '2012-11-10 14:37:03', 'System', 'Application started at 10-11-2012 15:37:03'),
(181, 0, -1, '2012-11-10 14:37:05', 'System', 'Loading content from disk cache...'),
(182, 0, -1, '2012-11-10 14:37:05', 'Custom', '[UmbracoExamine] Adding examine event handlers for index providers: 3'),
(183, 0, -1, '2012-11-10 14:37:30', 'Debug', 'return:developer/Macros/editMacro.aspx?macroID=2'),
(184, 0, 1062, '2012-11-10 14:37:44', 'Open', ''),
(185, 0, 1068, '2012-11-10 14:37:47', 'Open', ''),
(186, 0, -1, '2012-11-10 14:39:33', 'System', 'Application started at 10-11-2012 15:39:33'),
(187, 0, -1, '2012-11-10 14:39:34', 'System', 'Loading content from disk cache...'),
(188, 0, -1, '2012-11-10 14:39:34', 'Custom', '[UmbracoExamine] Adding examine event handlers for index providers: 3'),
(189, 0, 1068, '2012-11-10 14:39:37', 'Open', ''),
(190, 0, 1068, '2012-11-10 14:39:49', 'Save', ''),
(191, 0, -1, '2012-11-10 14:42:53', 'System', 'Application started at 10-11-2012 15:42:53'),
(192, 0, -1, '2012-11-10 14:42:54', 'System', 'Loading content from disk cache...'),
(193, 0, -1, '2012-11-10 14:42:54', 'Custom', '[UmbracoExamine] Adding examine event handlers for index providers: 3'),
(194, 0, 1068, '2012-11-10 14:43:09', 'Open', ''),
(195, 0, -1, '2012-11-10 14:43:51', 'System', 'Application started at 10-11-2012 15:43:51'),
(196, 0, -1, '2012-11-10 14:43:53', 'System', 'Loading content from disk cache...'),
(197, 0, -1, '2012-11-10 14:43:53', 'Custom', '[UmbracoExamine] Adding examine event handlers for index providers: 3'),
(198, 0, 1068, '2012-11-10 14:43:55', 'Open', ''),
(199, 0, -1, '2012-11-10 14:44:37', 'System', 'Application started at 10-11-2012 15:44:36'),
(200, 0, -1, '2012-11-10 14:44:38', 'System', 'Loading content from disk cache...'),
(201, 0, -1, '2012-11-10 14:44:38', 'Custom', '[UmbracoExamine] Adding examine event handlers for index providers: 3'),
(202, 0, 1068, '2012-11-10 14:44:41', 'Open', ''),
(203, 0, -1, '2012-11-10 14:46:02', 'System', 'Application started at 10-11-2012 15:46:01'),
(204, 0, -1, '2012-11-10 14:46:03', 'System', 'Loading content from disk cache...'),
(205, 0, -1, '2012-11-10 14:46:03', 'Custom', '[UmbracoExamine] Adding examine event handlers for index providers: 3'),
(206, 0, 1068, '2012-11-10 14:46:06', 'Open', ''),
(207, 0, 1068, '2012-11-10 14:46:13', 'Open', ''),
(208, 0, -1, '2012-11-10 14:46:35', 'System', 'Application started at 10-11-2012 15:46:35'),
(209, 0, -1, '2012-11-10 14:46:37', 'System', 'Loading content from disk cache...'),
(210, 0, -1, '2012-11-10 14:46:37', 'Custom', '[UmbracoExamine] Adding examine event handlers for index providers: 3'),
(211, 0, 1068, '2012-11-10 14:46:40', 'Open', ''),
(212, 0, -1, '2012-11-10 14:48:41', 'System', 'Application started at 10-11-2012 15:48:41'),
(213, 0, -1, '2012-11-10 14:48:42', 'System', 'Loading content from disk cache...'),
(214, 0, -1, '2012-11-10 14:48:42', 'Custom', '[UmbracoExamine] Adding examine event handlers for index providers: 3'),
(215, 0, 1068, '2012-11-10 14:48:48', 'Open', ''),
(216, 0, -1, '2012-11-10 15:46:41', 'Login', ''),
(217, 0, -1, '2012-11-10 15:48:22', 'Debug', 'return:developer/datatypes/editDataType.aspx?id=1075'),
(218, 0, 1059, '2012-11-10 15:49:39', 'Open', ''),
(219, 0, 1059, '2012-11-10 15:51:40', 'Open', ''),
(220, 0, 1059, '2012-11-10 15:52:00', 'Open', ''),
(221, 0, 1076, '2012-11-10 15:52:08', 'New', ''),
(222, 0, 1076, '2012-11-10 15:52:08', 'Open', ''),
(223, 0, 1076, '2012-11-10 15:52:54', 'Open', ''),
(224, 0, 1076, '2012-11-10 15:53:08', 'Open', ''),
(225, 0, 1076, '2012-11-10 15:54:00', 'Open', ''),
(226, 0, 1076, '2012-11-10 15:55:10', 'Open', ''),
(227, 0, 1076, '2012-11-10 15:59:03', 'Open', ''),
(228, 0, 1076, '2012-11-10 15:59:16', 'Save', ''),
(229, 0, 1076, '2012-11-10 15:59:26', 'Save', ''),
(230, 0, 1076, '2012-11-10 16:00:02', 'Save', ''),
(231, 0, 1076, '2012-11-10 16:00:07', 'Save', ''),
(232, 0, 1076, '2012-11-10 16:00:46', 'Open', ''),
(233, 0, 1078, '2012-11-10 16:00:55', 'New', ''),
(234, 0, 1078, '2012-11-10 16:00:55', 'Open', ''),
(235, 0, 1076, '2012-11-10 16:01:05', 'Open', ''),
(236, 0, 1076, '2012-11-10 16:01:08', 'Save', ''),
(237, 0, 1076, '2012-11-10 16:01:11', 'Save', ''),
(238, 0, 1078, '2012-11-10 16:01:13', 'Open', ''),
(239, 0, 1078, '2012-11-10 16:01:13', 'Save', ''),
(240, 0, 1078, '2012-11-10 16:01:13', 'Publish', ''),
(241, 0, -1, '2012-11-10 16:01:13', 'Debug', 'Republishing starting'),
(242, 0, -1, '2012-11-10 16:01:13', 'System', 'Loading content from database...'),
(243, 0, -1, '2012-11-10 16:01:13', 'Debug', 'Xml Pages loaded'),
(244, 0, -1, '2012-11-10 16:01:13', 'Debug', 'Done republishing Xml Index'),
(245, 0, 1067, '2012-11-10 16:01:13', 'Error', 'Error adding to SiteMapProvider in loadNodes(): System.InvalidOperationException: Multiple nodes with the same URL ''/'' were found. XmlSiteMapProvider requires that sitemap nodes have unique URLs.\r\n   at System.Web.StaticSiteMapProvider.AddNode(SiteMapNode node, SiteMapNode parentNode)\r\n   at umbraco.presentation.nodeFactory.UmbracoSiteMapProvider.loadNodes(String parentId, SiteMapNode parentNode)'),
(246, 0, -1, '2012-11-10 16:01:14', 'Debug', 'Xml saved in 00:00:00.0249674'),
(247, 0, 1076, '2012-11-10 16:01:16', 'Open', ''),
(248, 0, 1076, '2012-11-10 16:01:17', 'Save', ''),
(249, 0, 1076, '2012-11-10 16:01:17', 'Publish', ''),
(250, 0, -1, '2012-11-10 16:01:17', 'Debug', 'Xml saved in 00:00:00.0233825'),
(251, 0, 1078, '2012-11-10 16:01:20', 'Open', ''),
(252, 0, 1076, '2012-11-10 16:01:22', 'Open', ''),
(253, 0, 1062, '2012-11-10 16:01:47', 'Open', ''),
(254, 0, 1062, '2012-11-10 16:02:54', 'Save', ''),
(255, 0, 1062, '2012-11-10 16:02:57', 'Save', ''),
(256, 0, 1062, '2012-11-10 16:02:57', 'Publish', ''),
(257, 0, -1, '2012-11-10 16:02:58', 'Debug', 'Xml saved in 00:00:00.0192447'),
(258, 0, 1062, '2012-11-10 16:06:48', 'Save', ''),
(259, 0, 1062, '2012-11-10 16:06:48', 'Publish', ''),
(260, 0, -1, '2012-11-10 16:06:48', 'Debug', 'Xml saved in 00:00:00.0358996'),
(261, 0, -1, '2012-11-15 18:27:48', 'System', 'Application started at 15-11-2012 19:27:48'),
(262, 0, -1, '2012-11-15 18:27:55', 'System', 'Loading content from disk cache...'),
(263, 0, -1, '2012-11-15 18:27:55', 'Custom', '[UmbracoExamine] Adding examine event handlers for index providers: 3'),
(264, 0, -1, '2012-11-16 19:16:38', 'System', 'Application started at 16-11-2012 20:16:38'),
(265, 0, -1, '2012-11-16 19:16:44', 'System', 'Loading content from disk cache...'),
(266, 0, -1, '2012-11-16 19:16:45', 'Custom', '[UmbracoExamine] Adding examine event handlers for index providers: 3'),
(267, 0, -1, '2012-11-18 08:36:18', 'System', 'Application started at 18-11-2012 09:36:19'),
(268, 0, -1, '2012-11-18 08:36:23', 'System', 'Loading content from disk cache...'),
(269, 0, -1, '2012-11-18 08:36:23', 'Custom', '[UmbracoExamine] Adding examine event handlers for index providers: 3'),
(270, 0, -1, '2013-01-21 09:41:30', 'System', 'Application started at 21-01-2013 10:41:31'),
(271, 0, -1, '2013-01-21 09:41:32', 'System', 'Loading content from disk cache...'),
(272, 0, -1, '2013-01-21 09:41:32', 'Custom', '[UmbracoExamine] Adding examine event handlers for index providers: 3'),
(273, 0, -1, '2013-01-21 09:42:47', 'Login', '');

-- --------------------------------------------------------

--
-- Table structure for table `UMBRACONODE`
--

CREATE TABLE IF NOT EXISTS `UMBRACONODE` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `TRASHED` bit(1) NOT NULL DEFAULT b'0',
  `PARENTID` int(11) NOT NULL,
  `NODEUSER` int(11) DEFAULT NULL,
  `LEVEL` smallint(6) NOT NULL,
  `PATH` varchar(150) CHARACTER SET utf8 NOT NULL,
  `SORTORDER` int(11) NOT NULL,
  `UNIQUEID` char(36) COLLATE latin1_danish_ci DEFAULT NULL,
  `TEXT` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `NODEOBJECTTYPE` char(36) COLLATE latin1_danish_ci DEFAULT NULL,
  `CREATEDATE` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ID`),
  KEY `IX_UMBRACONODEPARENTID` (`PARENTID`),
  KEY `IX_UMBRACONODEOBJECTTYPE` (`NODEOBJECTTYPE`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=1079 ;

--
-- Dumping data for table `UMBRACONODE`
--

INSERT INTO `UMBRACONODE` (`ID`, `TRASHED`, `PARENTID`, `NODEUSER`, `LEVEL`, `PATH`, `SORTORDER`, `UNIQUEID`, `TEXT`, `NODEOBJECTTYPE`, `CREATEDATE`) VALUES
(-92, '\0', -1, 0, 11, '-1,-92', 37, 'f0bc4bfb-b499-40d6-ba86-058885a5178c', 'Label', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2004-09-30 12:01:49'),
(-90, '\0', -1, 0, 11, '-1,-90', 35, '84c6b441-31df-4ffe-b67e-67d5bc3ae65a', 'Upload', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2004-09-30 12:01:49'),
(-89, '\0', -1, 0, 11, '-1,-89', 34, 'c6bac0dd-4ab9-45b1-8e30-e4b619ee5da3', 'Textbox multiple', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2004-09-30 12:01:49'),
(-88, '\0', -1, 0, 11, '-1,-88', 33, '0cc0eba1-9960-42c9-bf9b-60e150b429ae', 'Textstring', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2004-09-30 12:01:49'),
(-87, '\0', -1, 0, 11, '-1,-87', 32, 'ca90c950-0aff-4e72-b976-a30b1ac57dad', 'Richtext editor', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2004-09-30 12:01:49'),
(-51, '\0', -1, 0, 11, '-1,-51', 4, '2e6d3631-066e-44b8-aec4-96f09099b2b5', 'Numeric', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2004-09-30 12:01:49'),
(-49, '\0', -1, 0, 11, '-1,-49', 2, '92897bc6-a5f3-4ffe-ae27-f2e7e33dda49', 'True/false', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2004-09-30 12:01:49'),
(-43, '\0', -1, 0, 1, '-1,-43', 2, 'fbaf13a8-4036-41f2-93a3-974f678c312a', 'Checkbox list', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2004-10-15 12:11:04'),
(-42, '\0', -1, 0, 1, '-1,-42', 2, '0b6a45e7-44ba-430d-9da5-4e46060b9e03', 'Dropdow', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2004-10-15 12:10:59'),
(-41, '\0', -1, 0, 1, '-1,-41', 2, '5046194e-4237-453c-a547-15db3a07c4e1', 'Date Picker', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2004-10-15 12:10:54'),
(-40, '\0', -1, 0, 1, '-1,-40', 2, 'bb5f57c9-ce2b-4bb9-b697-4caca783a805', 'Radiobox', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2004-10-15 12:10:49'),
(-39, '\0', -1, 0, 1, '-1,-39', 2, 'f38f0ac7-1d27-439c-9f3f-089cd8825a53', 'Dropdown multiple', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2004-10-15 12:10:44'),
(-38, '\0', -1, 0, 1, '-1,-38', 2, 'fd9f1447-6c61-4a7c-9595-5aa39147d318', 'Folder Browser', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2004-10-15 12:10:37'),
(-37, '\0', -1, 0, 1, '-1,-37', 2, '0225af17-b302-49cb-9176-b9f35cab9c17', 'Approved Color', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2004-10-15 12:10:30'),
(-36, '\0', -1, 0, 1, '-1,-36', 2, 'e4d66c0f-b935-4200-81f0-025f7256b89a', 'Date Picker with time', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2004-10-15 12:10:23'),
(-21, '\0', -1, 0, 0, '-1,-21', 0, 'BF7C7CBC-952F-4518-97A2-69E9C7B33842', 'Recycle Bin', 'CF3D8E34-1C1C-41e9-AE56-878B57B32113', '2009-08-27 22:28:28'),
(-20, '\0', -1, 0, 0, '-1,-20', 0, '0F582A79-1E41-4CF0-BFA0-76340651891A', 'Recycle Bin', '01BB7FF2-24DC-4C0C-95A2-C24EF72BBAC8', '2004-09-30 12:01:49'),
(-1, '\0', -1, 0, 0, '-1', 0, '916724a5-173d-4619-b97e-b9de133dd6f5', 'SYSTEM DATA: umbraco master root', 'ea7d8624-4cfe-4578-a871-24aa946bf34d', '2004-09-30 12:01:49'),
(1031, '\0', -1, 1, 1, '-1,1031', 2, 'f38bd2d7-65d0-48e6-95dc-87ce06ec2d3d', 'Folder', '4ea4382b-2f5a-4c2b-9587-ae9b3cf3602e', '2004-11-30 23:13:40'),
(1032, '\0', -1, 1, 1, '-1,1032', 2, 'cc07b313-0843-4aa8-bbda-871c8da728c8', 'Image', '4ea4382b-2f5a-4c2b-9587-ae9b3cf3602e', '2004-11-30 23:13:43'),
(1033, '\0', -1, 1, 1, '-1,1033', 2, '4c52d8ab-54e6-40cd-999c-7a5f24903e4d', 'File', '4ea4382b-2f5a-4c2b-9587-ae9b3cf3602e', '2004-11-30 23:13:46'),
(1034, '\0', -1, 0, 1, '-1,1034', 2, 'a6857c73-d6e9-480c-b6e6-f15f6ad11125', 'Content Picker', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2006-01-03 12:07:29'),
(1035, '\0', -1, 0, 1, '-1,1035', 2, '93929b9a-93a2-4e2a-b239-d99334440a59', 'Media Picker', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2006-01-03 12:07:36'),
(1036, '\0', -1, 0, 1, '-1,1036', 2, '2b24165f-9782-4aa3-b459-1de4a4d21f60', 'Member Picker', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2006-01-03 12:07:40'),
(1038, '\0', -1, 0, 1, '-1,1038', 2, '1251c96c-185c-4e9b-93f4-b48205573cbd', 'Simple Editor', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2006-01-03 12:07:55'),
(1039, '\0', -1, 0, 1, '-1,1039', 2, '06f349a9-c949-4b6a-8660-59c10451af42', 'Ultimate Picker', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2006-01-03 12:07:55'),
(1040, '\0', -1, 0, 1, '-1,1040', 2, '21e798da-e06e-4eda-a511-ed257f78d4fa', 'Related Links', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2006-01-03 12:07:55'),
(1041, '\0', -1, 0, 1, '-1,1041', 2, 'b6b73142-b9c1-4bf8-a16d-e1c23320b549', 'Tags', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2006-01-03 12:07:55'),
(1042, '\0', -1, 0, 1, '-1,1042', 2, '0a452bd5-83f9-4bc3-8403-1286e13fb77e', 'Macro Container', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2006-01-03 12:07:55'),
(1043, '\0', -1, 0, 1, '-1,1042', 2, '1df9f033-e6d4-451f-b8d2-e0cbc50a836f', 'Image Cropper', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2006-01-03 12:07:55'),
(1044, '\0', -1, 0, 1, '-1,1044', 0, '9121d939-fe4a-4a1e-b991-035d3e292d09', 'Administrator', '9b5416fb-e72f-45a9-a07b-5a9a2709ce43', '2012-11-09 15:46:00'),
(1045, '\0', -1, 0, 1, '-1,1045', 0, 'ce5081f9-7f28-43e4-a1f1-4decc1d6a8fc', 'Partner', '9b5416fb-e72f-45a9-a07b-5a9a2709ce43', '2012-11-09 15:46:05'),
(1046, '\0', -1, 0, 1, '-1,1046', 0, '8fa11424-5579-4579-9f59-a0c4a3e6918a', 'Optician', '9b5416fb-e72f-45a9-a07b-5a9a2709ce43', '2012-11-09 15:46:11'),
(1047, '\0', -1, 0, 1, '-1,1047', 0, '5a8c15c4-b73d-44d5-a4f3-778b82397210', 'Client', '9b5416fb-e72f-45a9-a07b-5a9a2709ce43', '2012-11-09 15:46:17'),
(1049, '\0', -1, 0, 1, '-1,1049', 0, 'f5a4fc9a-fbd6-465f-b309-e27a2a88ab16', 'Administrators', '366e63b9-880f-4e13-a61c-98069b029728', '2012-11-09 15:46:27'),
(1050, '\0', -1, 0, 1, '-1,1050', 0, 'f89bdb2e-60ee-494e-8f41-22669e538bab', 'Partners', '366e63b9-880f-4e13-a61c-98069b029728', '2012-11-09 15:46:32'),
(1051, '\0', -1, 0, 1, '-1,1051', 0, '336f97a9-3940-41de-8aab-f1843351d007', 'Opticians', '366e63b9-880f-4e13-a61c-98069b029728', '2012-11-09 15:46:37'),
(1052, '\0', -1, 0, 1, '-1,1052', 0, '4f23c59a-c439-41b6-bcd5-f37cfcedaa32', 'Clients', '366e63b9-880f-4e13-a61c-98069b029728', '2012-11-09 15:46:43'),
(1053, '\0', -1, 0, 1, '-1,1053', 0, '100dc924-e87f-4ab8-9a9e-1cb10f07c9d3', 'a', '39eb0f98-b348-42a1-8662-e7eb18487560', '2012-11-09 15:48:06'),
(1056, '\0', -1, 0, 1, '-1,1056', 0, 'a28c8a2a-fdaf-45d8-9014-c7ebf5fa911a', 'Exercise', 'a2cb7800-f571-4787-9638-bc48539a0efb', '2012-11-09 15:49:36'),
(1057, '\0', -1, 0, 1, '-1,1057', 0, '5f388769-2154-402d-82bd-d271f4c851b1', 'ExerciseInfo', 'a2cb7800-f571-4787-9638-bc48539a0efb', '2012-11-09 16:24:43'),
(1058, '\0', -1, 0, 1, '-1,1058', 0, 'df94b684-d044-4c90-b4c2-c97cc13ade42', 'ExerciseFolder', 'a2cb7800-f571-4787-9638-bc48539a0efb', '2012-11-09 16:36:35'),
(1059, '\0', -1, 0, 1, '-1,1059', 1, '0da46724-3e11-4883-8328-40d27319be61', 'Exercises', 'c66ba18e-eaf3-4cff-8a22-41b16d66a972', '2012-11-09 16:36:58'),
(1060, '\0', 1059, 0, 2, '-1,1059,1060', 0, '7f280f90-8a6f-4d8a-8c1c-2f408915a918', 'Vestibulærtræning', 'c66ba18e-eaf3-4cff-8a22-41b16d66a972', '2012-11-09 19:28:02'),
(1061, '\0', -1, 0, 1, '-1,1061', 1, '7e4afa4b-6495-4626-af66-cad15299fd70', 'ExerciseType', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2012-11-09 19:31:08'),
(1062, '\0', 1060, 0, 3, '-1,1059,1060,1062', 0, 'f6f39cc7-a426-4972-93f4-f959d1f75d34', 'Start', 'c66ba18e-eaf3-4cff-8a22-41b16d66a972', '2012-11-09 19:56:34'),
(1063, '\0', -1, 0, 1, '-1,1063', 1, 'd95c1537-b60a-41d1-a101-a3f3063c5724', 'master', 'a2cb7800-f571-4787-9638-bc48539a0efb', '2012-11-09 20:06:13'),
(1064, '\0', -1, 0, 1, '-1,1064', 1, '12a5fdf8-23fd-431d-ab8c-ba14eabba59b', 'master', '6fbde604-4178-42ce-a10b-8a2600a2f07d', '2012-11-09 20:06:14'),
(1065, '\0', -1, 0, 1, '-1,1065', 1, 'e420ffb5-4402-4804-81ea-070e2be4a596', 'page', 'a2cb7800-f571-4787-9638-bc48539a0efb', '2012-11-09 20:12:26'),
(1066, '\0', -1, 0, 1, '-1,1066', 1, 'f6620323-32d8-4c78-aad0-f58f4296954a', 'page', '6fbde604-4178-42ce-a10b-8a2600a2f07d', '2012-11-09 20:12:26'),
(1067, '\0', -1, 0, 1, '-1,1067', 0, '74374eec-e1ea-47b6-a947-ad4b351b96dd', 'Forside', 'c66ba18e-eaf3-4cff-8a22-41b16d66a972', '2012-11-09 20:13:41'),
(1068, '\0', 1067, 0, 2, '-1,1067,1068', 0, '6df3127a-9fa8-4842-8ddb-affcbc246d02', 'Test dine øjne', 'c66ba18e-eaf3-4cff-8a22-41b16d66a972', '2012-11-09 21:59:35'),
(1069, '\0', 1067, 0, 2, '-1,1067,1069', 1, 'ab38221f-bf8a-48dd-aacd-3402bed5f58a', 'TYE', 'c66ba18e-eaf3-4cff-8a22-41b16d66a972', '2012-11-09 22:08:03'),
(1070, '\0', -1, 0, 1, '-1,1070', 2, '1161b4a7-bcd2-4fe7-b4a1-f39e8d4e0256', 'Country Picker', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2012-11-10 07:24:28'),
(1071, '\0', -1, 0, 1, '-1,1071', 2, '7048e539-17e3-4052-beb1-a05a0a2343cc', 'aa - optician', '39eb0f98-b348-42a1-8662-e7eb18487560', '2012-11-10 07:28:03'),
(1073, '\0', -1, 0, 1, '-1,1073', 2, '60e0feba-07da-4ac3-9747-935b9cae994e', 'Cascading Dropdown Parent', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2012-11-10 07:45:21'),
(1074, '\0', -1, 0, 1, '-1,1074', 2, '4ba86de0-d6a3-4791-b739-733bf3c4fc9c', 'ExerciseLog', 'a2cb7800-f571-4787-9638-bc48539a0efb', '2012-11-10 15:47:43'),
(1075, '\0', -1, 0, 1, '-1,1075', 2, 'f4da4636-c5a9-4aca-9423-c110335ed13c', 'Exercise Picker', '30a2a501-1978-4ddb-a57b-f7efed43ba3c', '2012-11-10 15:48:22'),
(1076, '\0', 1078, 0, 2, '-1,1078,1076', 1, '841a608e-789e-488c-b0b9-4afc80619c94', 'log 1', 'c66ba18e-eaf3-4cff-8a22-41b16d66a972', '2012-11-10 15:52:08'),
(1077, '\0', -1, 0, 1, '-1,1077', 3, '00a34ed9-0b79-4064-8079-384fac2f7e2c', 'ExerciseLogFolder', 'a2cb7800-f571-4787-9638-bc48539a0efb', '2012-11-10 16:00:35'),
(1078, '\0', -1, 0, 1, '-1,1078', 3, '8eaf78ee-687a-447d-9ca9-fba50aa93e1d', 'Logs', 'c66ba18e-eaf3-4cff-8a22-41b16d66a972', '2012-11-10 16:00:54');

-- --------------------------------------------------------

--
-- Table structure for table `UMBRACORELATION`
--

CREATE TABLE IF NOT EXISTS `UMBRACORELATION` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `PARENTID` int(11) NOT NULL,
  `CHILDID` int(11) NOT NULL,
  `RELTYPE` int(11) NOT NULL,
  `DATETIME` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `COMMENT` varchar(1000) CHARACTER SET utf8 NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `UMBRACORELATIONTYPE`
--

CREATE TABLE IF NOT EXISTS `UMBRACORELATIONTYPE` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `DUAL` bit(1) NOT NULL,
  `PARENTOBJECTTYPE` char(36) COLLATE latin1_danish_ci NOT NULL,
  `CHILDOBJECTTYPE` char(36) COLLATE latin1_danish_ci NOT NULL,
  `NAME` varchar(255) CHARACTER SET utf8 NOT NULL,
  `ALIAS` varchar(100) CHARACTER SET utf8 DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=2 ;

--
-- Dumping data for table `UMBRACORELATIONTYPE`
--

INSERT INTO `UMBRACORELATIONTYPE` (`ID`, `DUAL`, `PARENTOBJECTTYPE`, `CHILDOBJECTTYPE`, `NAME`, `ALIAS`) VALUES
(1, '', 'c66ba18e-eaf3-4cff-8a22-41b16d66a972', 'c66ba18e-eaf3-4cff-8a22-41b16d66a972', 'Relate Document On Copy', 'relateDocumentOnCopy');

-- --------------------------------------------------------

--
-- Table structure for table `UMBRACOUSER`
--

CREATE TABLE IF NOT EXISTS `UMBRACOUSER` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `USERDISABLED` bit(1) NOT NULL DEFAULT b'0',
  `USERNOCONSOLE` bit(1) NOT NULL DEFAULT b'0',
  `USERTYPE` smallint(6) NOT NULL,
  `STARTSTRUCTUREID` int(11) NOT NULL,
  `STARTMEDIAID` int(11) DEFAULT NULL,
  `USERNAME` varchar(255) CHARACTER SET utf8 NOT NULL,
  `USERLOGIN` varchar(125) CHARACTER SET utf8 NOT NULL,
  `USERPASSWORD` varchar(125) CHARACTER SET utf8 NOT NULL,
  `USEREMAIL` varchar(255) CHARACTER SET utf8 NOT NULL,
  `USERDEFAULTPERMISSIONS` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `USERLANGUAGE` varchar(10) CHARACTER SET utf8 DEFAULT NULL,
  `DEFAULTTOLIVEEDITING` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`ID`),
  KEY `USERTYPE` (`USERTYPE`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=1 ;

--
-- Dumping data for table `UMBRACOUSER`
--

INSERT INTO `UMBRACOUSER` (`ID`, `USERDISABLED`, `USERNOCONSOLE`, `USERTYPE`, `STARTSTRUCTUREID`, `STARTMEDIAID`, `USERNAME`, `USERLOGIN`, `USERPASSWORD`, `USEREMAIL`, `USERDEFAULTPERMISSIONS`, `USERLANGUAGE`, `DEFAULTTOLIVEEDITING`) VALUES
(0, '\0', '\0', 1, -1, -1, 'monosolutions', 'monosolutions', '+aGEv2Xno4MEOoH6mcz+2TUkXQs=', 'contact@monosolutions.dk', NULL, 'en', '\0');

-- --------------------------------------------------------

--
-- Table structure for table `UMBRACOUSER2APP`
--

CREATE TABLE IF NOT EXISTS `UMBRACOUSER2APP` (
  `USER` int(11) NOT NULL,
  `APP` varchar(50) CHARACTER SET utf8 NOT NULL,
  PRIMARY KEY (`USER`,`APP`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci;

--
-- Dumping data for table `UMBRACOUSER2APP`
--

INSERT INTO `UMBRACOUSER2APP` (`USER`, `APP`) VALUES
(0, 'content'),
(0, 'developer'),
(0, 'media'),
(0, 'member'),
(0, 'settings'),
(0, 'translation'),
(0, 'users');

-- --------------------------------------------------------

--
-- Table structure for table `UMBRACOUSER2NODENOTIFY`
--

CREATE TABLE IF NOT EXISTS `UMBRACOUSER2NODENOTIFY` (
  `USERID` int(11) NOT NULL,
  `NODEID` int(11) NOT NULL,
  `ACTION` char(1) COLLATE latin1_danish_ci NOT NULL,
  PRIMARY KEY (`USERID`,`NODEID`,`ACTION`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci;

-- --------------------------------------------------------

--
-- Table structure for table `UMBRACOUSER2NODEPERMISSION`
--

CREATE TABLE IF NOT EXISTS `UMBRACOUSER2NODEPERMISSION` (
  `USERID` int(11) NOT NULL,
  `NODEID` int(11) NOT NULL,
  `PERMISSION` char(1) COLLATE latin1_danish_ci NOT NULL,
  PRIMARY KEY (`USERID`,`NODEID`,`PERMISSION`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci;

-- --------------------------------------------------------

--
-- Table structure for table `UMBRACOUSERGROUP`
--

CREATE TABLE IF NOT EXISTS `UMBRACOUSERGROUP` (
  `ID` smallint(6) NOT NULL AUTO_INCREMENT,
  `USERGROUPNAME` varchar(255) CHARACTER SET utf8 NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `UMBRACOUSERLOGINS`
--

CREATE TABLE IF NOT EXISTS `UMBRACOUSERLOGINS` (
  `CONTEXTID` char(36) COLLATE latin1_danish_ci NOT NULL,
  `USERID` int(11) NOT NULL,
  `TIMEOUT` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci;

--
-- Dumping data for table `UMBRACOUSERLOGINS`
--

INSERT INTO `UMBRACOUSERLOGINS` (`CONTEXTID`, `USERID`, `TIMEOUT`) VALUES
('3e354dd7-6bfb-4b27-9c3c-3387ca7f54e3', 0, 634880778585244569),
('78fe3248-1e97-476c-a12d-3067f9630797', 0, 634881611852617798),
('f0575f81-9d60-42be-b03e-4004e868644f', 0, 634881652128607728),
('056a98b6-f20a-456a-87c3-eb5a2e1b45e2', 0, 634943637523685852);

-- --------------------------------------------------------

--
-- Table structure for table `UMBRACOUSERTYPE`
--

CREATE TABLE IF NOT EXISTS `UMBRACOUSERTYPE` (
  `ID` smallint(6) NOT NULL AUTO_INCREMENT,
  `USERTYPEALIAS` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `USERTYPENAME` varchar(255) CHARACTER SET utf8 NOT NULL,
  `USERTYPEDEFAULTPERMISSIONS` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_danish_ci AUTO_INCREMENT=5 ;

--
-- Dumping data for table `UMBRACOUSERTYPE`
--

INSERT INTO `UMBRACOUSERTYPE` (`ID`, `USERTYPEALIAS`, `USERTYPENAME`, `USERTYPEDEFAULTPERMISSIONS`) VALUES
(1, 'admin', 'Administrators', 'CADMOSKTPIURZ5:F'),
(2, 'writer', 'Writer', 'CAH:F'),
(3, 'editor', 'Editors', 'CADMOSKTPUZ5:F'),
(4, 'translator', 'Translator', 'AF');

--
-- Constraints for dumped tables
--

--
-- Constraints for table `CMSCONTENT`
--
ALTER TABLE `CMSCONTENT`
  ADD CONSTRAINT `CMSCONTENT_ibfk_1` FOREIGN KEY (`NODEID`) REFERENCES `UMBRACONODE` (`ID`);

--
-- Constraints for table `CMSCONTENTTYPE`
--
ALTER TABLE `CMSCONTENTTYPE`
  ADD CONSTRAINT `CMSCONTENTTYPE_ibfk_1` FOREIGN KEY (`NODEID`) REFERENCES `UMBRACONODE` (`ID`);

--
-- Constraints for table `CMSDOCUMENT`
--
ALTER TABLE `CMSDOCUMENT`
  ADD CONSTRAINT `CMSDOCUMENT_ibfk_1` FOREIGN KEY (`NODEID`) REFERENCES `UMBRACONODE` (`ID`);

--
-- Constraints for table `CMSMACROPROPERTY`
--
ALTER TABLE `CMSMACROPROPERTY`
  ADD CONSTRAINT `CMSMACROPROPERTY_ibfk_1` FOREIGN KEY (`MACROPROPERTYTYPE`) REFERENCES `CMSMACROPROPERTYTYPE` (`ID`);

--
-- Constraints for table `CMSPROPERTYDATA`
--
ALTER TABLE `CMSPROPERTYDATA`
  ADD CONSTRAINT `CMSPROPERTYDATA_ibfk_1` FOREIGN KEY (`CONTENTNODEID`) REFERENCES `UMBRACONODE` (`ID`);

--
-- Constraints for table `CMSPROPERTYTYPE`
--
ALTER TABLE `CMSPROPERTYTYPE`
  ADD CONSTRAINT `CMSPROPERTYTYPE_ibfk_1` FOREIGN KEY (`TABID`) REFERENCES `CMSTAB` (`ID`);

--
-- Constraints for table `CMSTAGRELATIONSHIP`
--
ALTER TABLE `CMSTAGRELATIONSHIP`
  ADD CONSTRAINT `CMSTAGS_CMSTAGRELATIONSHIP` FOREIGN KEY (`TAGID`) REFERENCES `CMSTAGS` (`ID`) ON DELETE CASCADE,
  ADD CONSTRAINT `UMBRACONODE_CMSTAGRELATIONSHIP` FOREIGN KEY (`NODEID`) REFERENCES `UMBRACONODE` (`ID`) ON DELETE CASCADE;

--
-- Constraints for table `CMSTEMPLATE`
--
ALTER TABLE `CMSTEMPLATE`
  ADD CONSTRAINT `CMSTEMPLATE_ibfk_1` FOREIGN KEY (`NODEID`) REFERENCES `UMBRACONODE` (`ID`);

--
-- Constraints for table `UMBRACONODE`
--
ALTER TABLE `UMBRACONODE`
  ADD CONSTRAINT `UMBRACONODE_ibfk_1` FOREIGN KEY (`PARENTID`) REFERENCES `UMBRACONODE` (`ID`);

--
-- Constraints for table `UMBRACOUSER`
--
ALTER TABLE `UMBRACOUSER`
  ADD CONSTRAINT `UMBRACOUSER_ibfk_1` FOREIGN KEY (`USERTYPE`) REFERENCES `UMBRACOUSERTYPE` (`ID`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
