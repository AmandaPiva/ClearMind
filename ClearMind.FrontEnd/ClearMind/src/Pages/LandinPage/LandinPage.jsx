import logo from "../../../src/assets/logoClearmind (1).png";

function LandinPage() {
  return (
    <>
      <div>
        <div className="h-5 w-full">
          <img src={logo} alt="logo" className="w-45 m-8" />
        </div>

        <div className="flex justify-center mt-70 flex-col items-center">
          <h1 className="text-2xl font-bold font-inter">
            Welcome to ClearMind!{" "}
          </h1>
          <p className="m-5 text-base">
            Hear, you can understand your feelings and resolve your problems
            <p className="text-center">with good advices for your situation.</p>
          </p>
          <h2 className="mt-30 text-lg font-bold">
            Sing up or log in to try the platform
          </h2>
        </div>
      </div>
    </>
  );
}

export default LandinPage;
