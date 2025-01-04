import useMessageService from "../hooks/useMessageService";

export default function Home() {
    const messageService = useMessageService();

    return (
        <>
            <div className="flex flex-col items-center justify-center min-h-screen bg-gray-900 text-white">
                <h1 className="text-4xl font-bold mb-4">
                    Welcome to Deepslate Minecraft Launcher
                </h1>
                <p className="text-lg mb-8">
                    Launch and manage your Minecraft instances with ease.
                </p>
                <div className="space-x-4">
                    <button className="bg-green-500 hover:bg-green-700 text-white font-bold py-2 px-4 rounded"
                        onClick={() => {
                            messageService.sendPing(new Date().getTime()).then((response) => {
                                alert(response.nonce);
                            });
                        }}
                    >
                        Ping Back-End
                    </button>
                </div>
            </div>
        </>
    );
}
