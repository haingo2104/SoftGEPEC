import Image from "next/image";
import SideBar from "./sidebar/page";

export default function Home() {
    return (
        <div>
            <SideBar/>
            <main className="flex min-h-screen flex-col items-center justify-between p-24">
                <h1>Hello world!</h1>
            </main>
        </div>

    );
}
