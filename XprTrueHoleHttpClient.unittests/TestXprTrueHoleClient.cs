using Moq;
using NUnit.Framework;

namespace Hypertherm.XprTrueHoleHttpClient.unittests
{
    [TestFixture]
    public class TestXprTrueHoleClient
    {
        XprTrueHoleClient thClient;
        string settings;
        string part;
        string output;
        string settingsID;
        string partfileID;
        string convertedfileID;
        Mock<ITHApiClient> httpClientMock;

        [SetUp]
        protected void Setup()
        {
            httpClientMock = new Mock<ITHApiClient>();
            thClient = new XprTrueHoleClient(httpClientMock.Object);
            settings = "settings content";
            part = "part content";
            output = "output content";
            settingsID = "settingsID";
            partfileID = "partfileID";
            convertedfileID = "convertedfileID";

        }

        [Test]
        public void SuccessfullyConvert()
        {
            httpClientMock.Setup(hcm => hcm.Upload(settings)).Returns(settingsID);
            httpClientMock.Setup(hcm => hcm.Upload(part)).Returns(partfileID);
            httpClientMock.Setup(hcm => hcm.Convert(settingsID,partfileID)).Returns(convertedfileID);
            httpClientMock.Setup(hcm => hcm.Download(convertedfileID)).Returns(output);

            var outputContent = thClient.Convert(settings, part);

            Assert.AreEqual(output, outputContent);
        }
    }
}
