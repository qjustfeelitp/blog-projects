using Microsoft.Extensions.DependencyInjection;

namespace LazyDependency;

//internal static class Bad
//{
//    internal sealed class A
//    {
//        public void DoSomething()
//        {
//            var b = new B();

//            b.DoSomethingElse();
//        }
//    }

//    internal sealed class B
//    {
//        public void DoSomethingElse()
//        {
//            var a = new A();
//        }
//    }
//}

//internal static class Bad
//{
//    internal sealed class A
//    {
//        private readonly B b;

//        public A(B b)
//        {
//            this.b = b;
//        }
//    }

//    internal sealed class B
//    {
//        private readonly A a;

//        public B(A a)
//        {
//            this.a = a;
//        }
//    }
//}

internal static class Bad
{
    [Service(ServiceLifetime.Transient)]
    internal sealed class A
    {
        private readonly Lazy<B> b;

        public A(Lazy<B> b)
        {
            this.b = b;
        }
    }

    [Service(ServiceLifetime.Transient)]
    internal sealed class B
    {
        private readonly A a;

        public B(A a)
        {
            this.a = a;
        }
    }

    [Service(ServiceLifetime.Transient)]
    internal sealed class C
    {
        private readonly Lazy<A> lazy;
        private readonly Func<A> func;

        public C(Lazy<A> lazy, Func<A> func)
        {
            this.lazy = lazy;
            this.func = func;
        }
    }
}

